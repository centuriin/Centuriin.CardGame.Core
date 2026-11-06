namespace Centuriin.CardGame.Core.Common.Root.State;

public interface IGameState : IReadOnlyGameState
{
    public new ITurnAutomat TurnAutomat { get; }

    public new IEntitiesCollection Entities { get; }

    public void ChangeStatus(GameStatus newStatus);

    public IReadOnlyGameState AsReadOnly();
}
