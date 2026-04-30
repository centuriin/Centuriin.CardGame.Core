namespace Centuriin.CardGame.Core.Common.World;

public interface IGameSnapshot
{
    public GameId GameId { get; }

    public GameStatus GameStatus { get; }

    public IGameState GameState { get; }

    public GameVersion Version { get; }
}