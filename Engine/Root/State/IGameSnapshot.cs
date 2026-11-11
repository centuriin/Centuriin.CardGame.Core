namespace Centuriin.CardGame.Core.Engine.Root.State;

public interface IGameSnapshot
{
    public GameId GameId { get; }

    public GameTypeId TypeId { get; }

    public GameStatus GameStatus { get; }

    public GameVersion Version { get; }

    public IReadOnlyEntitiesCollection Entities { get; }
}