using System.Threading.Channels;

using Centuriin.CardGame.Core.Common.Commands;
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

    private readonly ICommandValidator _commandValidator;
    private readonly IGameEventsRepository _eventsRepository;
    private readonly IEventDispatcher _dispatcher;

    public GameId GameId { get; }

    public IGameState State { get; }

    public Game(
        GameId gameId,
        IGameState gameState,
        ICommandValidator commandValidator,
        IGameEventsRepository eventsRepository,
        IEventDispatcher dispatcher)
    {
        GameId = gameId;

        ArgumentNullException.ThrowIfNull(gameState);
        State = gameState;

        ArgumentNullException.ThrowIfNull(commandValidator);
        _commandValidator = commandValidator;

        ArgumentNullException.ThrowIfNull(eventsRepository);
        _eventsRepository = eventsRepository;

        ArgumentNullException.ThrowIfNull(dispatcher);
        _dispatcher = dispatcher;

        _writer = new(_channel.Writer);
    }

    public async Task<ICommandResult> ExecuteAsync(ICommand command, CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(command);

        token.ThrowIfCancellationRequested();

        using var _ = Telemetry.StartActivity(command);

        var @event = _commandValidator.Validate(@command);

        if (@event is null)
        {
            // todo fail command
            return null!;
        }

        var eventUnit = ApplyCore(@event);

        await _eventsRepository.AddAsync(eventUnit, token);

        // todo make result
        return null!;
    }

    public async Task ApplyAsync(IPrimaryEvent @event, CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(@event);

        token.ThrowIfCancellationRequested();

        using var _ = Telemetry.StartActivity(@event);

        var eventUnit = ApplyCore(@event);

        await _eventsRepository.AddAsync(eventUnit, token);
    }

    private IGameEventUnit ApplyCore(IPrimaryEvent primaryEvent)
    {
        var randomEvents = new List<IRandomEvent>();

        _dispatcher.Publish(primaryEvent, State, _writer);

        while (_channel.Reader.TryRead(out var nextEvent))
        {
            if (nextEvent is IRandomEvent randomEvent)
            {
                randomEvents.Add(randomEvent);
            }

            _dispatcher.Publish(nextEvent, State, _writer);
        }

        return new EventUnit(primaryEvent, randomEvents);
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

    private sealed record class EventUnit(
        IPrimaryEvent PrimaryEvent,
        IReadOnlyCollection<IRandomEvent> RelatedRandomEvents) : IGameEventUnit;
}