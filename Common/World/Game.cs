using System.Threading.Channels;

using Centuriin.CardGame.Core.Common.Events;
using Centuriin.CardGame.Core.Common.Events.Dispatching;
using Centuriin.CardGame.Core.Common.Observability;
using Centuriin.CardGame.Core.Common.Repositories;

namespace Centuriin.CardGame.Core.Common.World;

public sealed class Game : IGame
{
    private readonly Channel<IGameEvent> _channel = Channel.CreateUnbounded<IGameEvent>(new()
    {
        SingleReader = true,
        SingleWriter = true,
    });

    private readonly ChannelWrapper _writer;

    private readonly IGameEventsRepository _eventsRepository;
    private readonly IEventDispatcher _dispatcher;

    public GameId GameId { get; }

    public IGameState State { get; }

    public Game(
        GameId gameId,
        IGameState gameState,
        IGameEventsRepository eventsRepository,
        IEventDispatcher dispatcher)
    {
        GameId = gameId;

        ArgumentNullException.ThrowIfNull(gameState);
        State = gameState;

        ArgumentNullException.ThrowIfNull(eventsRepository);
        _eventsRepository = eventsRepository;

        ArgumentNullException.ThrowIfNull(dispatcher);
        _dispatcher = dispatcher;

        _writer = new(_channel.Writer);
    }

    public async Task ApplyAsync(IGameEvent @event, CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(@event);

        token.ThrowIfCancellationRequested();

        using var _ = Telemetry.StartActivity(@event);

        _dispatcher.Publish(@event, State, _writer);

        while (_channel.Reader.TryRead(out var nextEvent))
        {
            using var __ = Telemetry.StartActivity(nextEvent);

            await _eventsRepository.AddAsync(nextEvent, token);

            _dispatcher.Publish(nextEvent, State, _writer);
        }
    }

    private sealed class ChannelWrapper : IEventBusWriter
    {
        private ChannelWriter<IGameEvent> Writer { get; }

        public ChannelWrapper(ChannelWriter<IGameEvent> writer)
        {
            Writer = writer;
        }

        public void Write(IGameEvent @event) => _ = Writer.TryWrite(@event);
    }
}
