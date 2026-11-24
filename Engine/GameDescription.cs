using Centuriin.CardGame.Core.Application;

namespace Centuriin.CardGame.Core.Engine;

public sealed class GameDescription
{
    public GameId GameId { get; }

    public GameTypeId GameTypeId { get; }

    public IGameSettings GameSettings { get; }

    public IReadOnlyList<PlayerId> PlayerIds { get; }

    public GameDescription(
        GameId gameId,
        GameTypeId gameTypeId, 
        IGameSettings gameSettings,
        IReadOnlyList<PlayerId> playerIds)
    {
        GameId = gameId;
        GameTypeId = gameTypeId;

        ArgumentNullException.ThrowIfNull(gameSettings);
        GameSettings = gameSettings;

        ArgumentNullException.ThrowIfNull(playerIds);
        PlayerIds = playerIds;
    }
}