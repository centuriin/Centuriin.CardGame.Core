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

    private readonly ChannelWrapper _writer;

    private readonly IGameEventsRepository _eventsRepository;
    private readonly IEventDispatcher _dispatcher;

    public IGameState State { get; }

    public Game(
        IGameState gameState,
        IGameEventsRepository eventsRepository,
        IEventDispatcher dispatcher)
    {
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

        _writer.Write(@event);

        while (_channel.Reader.TryRead(out var nextEvent))
        {
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
