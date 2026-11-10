using Centuriin.CardGame.Core.Engine.Commands;
using Centuriin.CardGame.Core.Engine.Events;
using Centuriin.CardGame.Core.Engine.Root.State;

namespace Centuriin.CardGame.Core.Engine.Root;

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

    protected override PreparedResult StartCore()
        => new(Version);

    protected override PreparedResult ExecuteCore(ICommand command)
        => new(Version);
}
