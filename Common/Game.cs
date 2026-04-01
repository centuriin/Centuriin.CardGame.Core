using System.Threading.Channels;

using Centuriin.CardGame.Core.Common.Events;
using Centuriin.CardGame.Core.Common.Repositories;

namespace Centuriin.CardGame.Core.Common;

public sealed class Game : IGame
{
    private readonly Channel<IGameEvent> _channel = Channel.CreateUnbounded<IGameEvent>(new()
    {
        SingleReader = true,
        SingleWriter = true,
    });

    private readonly IGameEventsRepository _eventsRepository;
    private readonly IEventDispatcher<IGameEvent> _dispatcher;

    public IGameState State { get; }

    public Game(
        IGameState gameState,
        IGameEventsRepository eventsRepository,
        IEventDispatcher<IGameEvent> dispatcher)
    {
        ArgumentNullException.ThrowIfNull(gameState);
        State = gameState;

        ArgumentNullException.ThrowIfNull(eventsRepository);
        _eventsRepository = eventsRepository;

        ArgumentNullException.ThrowIfNull(dispatcher);
        _dispatcher = dispatcher;
    }

    public async Task ApplyAsync(IGameEvent @event, CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(@event);

        token.ThrowIfCancellationRequested();

        _ = _channel.Writer.TryWrite(@event);

        while (_channel.Reader.TryRead(out var nextEvent))
        {
            await _eventsRepository.AddAsync(nextEvent, token);

            await _dispatcher.PublishAsync(nextEvent, _channel.Writer, token);
        }
    }
}
