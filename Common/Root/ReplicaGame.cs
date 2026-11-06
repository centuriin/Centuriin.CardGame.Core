using Centuriin.CardGame.Core.Common.Commands;
using Centuriin.CardGame.Core.Common.Events;
using Centuriin.CardGame.Core.Common.Events.Application;
using Centuriin.CardGame.Core.Common.Root.State;

namespace Centuriin.CardGame.Core.Common.Root;

public sealed class ReplicaGame : GameBase
{
    public ReplicaGame(
        GameId id,
        IGameState state,
        IGameEventApplier eventApplier,
        ICommandValidator commandValidator)
        : base(id, state, eventApplier, commandValidator)
    {
    }

    protected override IGameApplicationEvent StartCore()
        => new GamePreparedEvent(Id, Version);

    protected override IGameApplicationEvent ExecuteCore(ICommand command)
        => new GameCommandPreparedEvent(Id, Version, command);
}
