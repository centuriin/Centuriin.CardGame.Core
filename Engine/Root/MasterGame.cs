using Centuriin.CardGame.Core.Engine.Commands;
using Centuriin.CardGame.Core.Engine.Events;
using Centuriin.CardGame.Core.Engine.Events.Domain;
using Centuriin.CardGame.Core.Engine.Root.State;

namespace Centuriin.CardGame.Core.Engine.Root;

public sealed class MasterGame : GameBase
{
    private readonly IGameEventProducer _eventProducer;

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

    protected override ExecutedResult StartCore()
    {
        var batch = new GameEventsBatch(
            ++Version,
            _eventProducer.Produce(new GameStartedEvent(Id)));

        return new(batch);
    }

    protected override ExecutedResult ExecuteCore(ICommand command)
    {
        var @event = command.YieldGameEvent();

        var batch = new GameEventsBatch(
            ++Version,
            _eventProducer.Produce(@event));

        return new(batch);
    }
}
