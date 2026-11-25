using Centuriin.CardGame.Core.Engine.Commands;
using Centuriin.CardGame.Core.Engine.Events;
using Centuriin.CardGame.Core.Engine.Events.Domain;
using Centuriin.CardGame.Core.Engine.Root.State;

namespace Centuriin.CardGame.Core.Engine.Root;

/// <summary>
/// Produces and applies authoritative event batches for startup and accepted commands.
/// </summary>
public sealed class MasterGame : GameBase
{
    private readonly IGameEventProducer _eventProducer;

    /// <summary>
    /// Initializes a new <see cref="MasterGame"/> instance.
    /// </summary>
    /// <param name="id">
    /// The game identifier.
    /// </param>
    /// <param name="state">
    /// The mutable game state.
    /// </param>
    /// <param name="eventApplier">
    /// The service used to apply recorded events.
    /// </param>
    /// <param name="eventProducer">
    /// The service used to produce and apply successor events.
    /// </param>
    /// <param name="commandValidator">
    /// The validator used to check commands before execution.
    /// </param>
    public MasterGame(
        GameId id,
        IGameState state,
        IGameEventApplier eventApplier,
        IGameEventProducer eventProducer,
        ICommandValidator commandValidator)
        : base(id, state, eventApplier, commandValidator)
    {
        ArgumentNullException.ThrowIfNull(eventProducer);
        _eventProducer = eventProducer;
    }

    /// <inheritdoc/>
    protected override ExecutedResult StartCore()
    {
        var batch = new GameEventsBatch(
            ++Version,
            _eventProducer.Produce(new GameStartedEvent(Id)));

        return new(batch);
    }

    /// <inheritdoc/>
    protected override ExecutedResult ExecuteCore(ICommand command)
    {
        var @event = command.YieldGameEvent();

        var batch = new GameEventsBatch(
            ++Version,
            _eventProducer.Produce(@event));

        return new(batch);
    }
}
