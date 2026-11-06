using Centuriin.CardGame.Core.Common.Commands;
using Centuriin.CardGame.Core.Common.Events;
using Centuriin.CardGame.Core.Common.Events.Application;
using Centuriin.CardGame.Core.Common.Events.Domain;
using Centuriin.CardGame.Core.Common.Root.State;

namespace Centuriin.CardGame.Core.Common.Root;

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

    protected override IGameApplicationEvent StartCore()
    {
        var batch = new GameEventsBatch(
            ++Version,
            _eventProducer.Produce(new GameStartedEvent(Id)));

        return new GameRunningEvent(Id, batch);
    }

    protected override IGameApplicationEvent ExecuteCore(ICommand command)
    {
        var @event = command.YieldGameEvent();

        var batch = new GameEventsBatch(
            ++Version,
            _eventProducer.Produce(@event));

        return new NewGameEventsBatchCreatedEvent(Id, batch);
    }
}
