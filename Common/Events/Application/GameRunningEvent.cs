using Centuriin.CardGame.Core.Common.Events.Domain;
using Centuriin.CardGame.Core.Common.World;

namespace Centuriin.CardGame.Core.Common.Events.Application;

public sealed class GameRunningEvent : IApplicationEvent
{
    public GameId GameId { get; }

    public GameVersion GameVersion { get; }

    public IGameEventsBatch InitialEventsBatch { get; }

    public GameRunningEvent(
        GameId gameId, 
        GameVersion gameVersion, 
        IGameEventsBatch initialEventsBatch)
    {
        GameId = gameId;
        GameVersion = gameVersion;

        ArgumentNullException.ThrowIfNull(initialEventsBatch);
        InitialEventsBatch = initialEventsBatch;
    }
}
