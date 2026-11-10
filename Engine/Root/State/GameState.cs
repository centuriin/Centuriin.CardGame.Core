using System.ComponentModel;

namespace Centuriin.CardGame.Core.Engine.Root.State;

public sealed class GameState : IGameState
{
    public GameStatus Status { get; private set; } = GameStatus.Created;

    public ITurnAutomat TurnAutomat { get; }

    IReadOnlyTurnAutomat IReadOnlyGameState.TurnAutomat => TurnAutomat;

    public IEntitiesCollection Entities { get; } = new EntitiesCollection();

    IReadOnlyEntitiesCollection IReadOnlyGameState.Entities => Entities;

    public GameState(ITurnAutomat turnAutomat)
    {
        ArgumentNullException.ThrowIfNull(turnAutomat);
        TurnAutomat = turnAutomat;
    }

    public void ChangeStatus(GameStatus newStatus)
    {
        if (!Enum.IsDefined(newStatus))
        {
            throw new InvalidEnumArgumentException(
                nameof(newStatus),
                (int)newStatus,
                typeof(GameStatus));
        }

        Status = newStatus;
    }

    public IReadOnlyGameState AsReadOnly() => this;
}