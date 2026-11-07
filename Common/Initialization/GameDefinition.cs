using Centuriin.CardGame.Core.Common.Context;

namespace Centuriin.CardGame.Core.Common.Initialization;

public sealed class GameDefinition
{
    public GameId GameId { get; }

    public GameTypeId GameTypeId { get; }

    public IReadOnlyList<PlayerId> PlayerIds { get; }

    public IInitialStateContext StateContext { get; }

    public GameDefinition(
        GameId gameId,
        GameTypeId gameTypeId,
        IReadOnlyList<PlayerId> playerIds,
        IInitialStateContext initialStateContext)
    {
        GameId = gameId;
        GameTypeId = gameTypeId;

        ArgumentNullException.ThrowIfNull(playerIds);
        PlayerIds = playerIds;

        ArgumentNullException.ThrowIfNull(initialStateContext);
        StateContext = initialStateContext;
    }
}
