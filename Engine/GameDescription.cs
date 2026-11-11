namespace Centuriin.CardGame.Core.Engine;

public sealed class GameDescription
{
    public GameId GameId { get; }

    public GameTypeId GameTypeId { get; }

    public IReadOnlyList<PlayerId> PlayerIds { get; }

    public GameDescription(
        GameId gameId,
        GameTypeId gameTypeId, 
        IReadOnlyList<PlayerId> playerIds)
    {
        GameId = gameId;
        GameTypeId = gameTypeId;

        ArgumentNullException.ThrowIfNull(playerIds);
        PlayerIds = playerIds;
    }
}