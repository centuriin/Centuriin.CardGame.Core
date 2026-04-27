namespace Centuriin.CardGame.Core.Common.World;

public sealed record class GameSetup
{
    public GameTypeId GameTypeId { get; }

    public IReadOnlyCollection<PlayerId> PlayerIds { get; }

    public GameSetup(GameTypeId gameTypeId, IReadOnlyCollection<PlayerId> playerIds)
    {
        GameTypeId = gameTypeId;

        ArgumentNullException.ThrowIfNull(playerIds);
        PlayerIds = playerIds;
    }
}