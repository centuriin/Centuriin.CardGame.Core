namespace Centuriin.CardGame.Core.Common.Root.State;

public interface IReadOnlyGameState
{
    public GameStatus Status { get; }

    /// <summary>
    /// Read only turn automat.
    /// </summary>
    public IReadOnlyTurnAutomat TurnAutomat { get; }

    /// <summary>
    /// Read only entities collection.
    /// </summary>
    public IReadOnlyEntitiesCollection Entities { get; }
}
