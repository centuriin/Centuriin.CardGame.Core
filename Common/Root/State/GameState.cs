namespace Centuriin.CardGame.Core.Common.Root.State;

public sealed class GameState : IGameState
{
    public IEntitiesCollection Entities { get; } = new EntitiesCollection();

    IReadOnlyEntitiesCollection IReadOnlyGameState.Entities => Entities;

    public ITurnAutomat TurnAutomat { get; }

    IReadOnlyTurnAutomat IReadOnlyGameState.TurnAutomat => TurnAutomat;

    public GameState(ITurnAutomat turnAutomat)
    {
        ArgumentNullException.ThrowIfNull(turnAutomat);
        TurnAutomat = turnAutomat;
    }

    public IReadOnlyGameState AsReadOnly() => this;
}