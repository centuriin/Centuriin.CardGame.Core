using Centuriin.CardGame.Core.Engine;
using Centuriin.CardGame.Core.Engine.Context;

namespace Application.Initialization;

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
