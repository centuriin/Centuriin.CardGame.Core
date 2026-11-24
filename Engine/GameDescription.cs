using Centuriin.CardGame.Core.Application;

namespace Centuriin.CardGame.Core.Engine;

public sealed class GameDescription
{
    public GameId GameId { get; }

    public GameTypeId GameTypeId { get; }

    public IGameSettings GameSettings { get; }

    public GameDescription(
        GameId gameId,
        GameTypeId gameTypeId, 
        IGameSettings gameSettings)
    {
        GameId = gameId;
        GameTypeId = gameTypeId;

        ArgumentNullException.ThrowIfNull(gameSettings);
        GameSettings = gameSettings;
    }
}