using Centuriin.CardGame.Core.Common.Events.Domain;
using Centuriin.CardGame.Core.Common.World;

namespace Centuriin.CardGame.Core.Common.Events.Application;

public sealed class GameRunningEvent : IGameApplicationEvent
{
    public GameId GameId { get; }

    public IGameEventsBatch InitialEventsBatch { get; }

    public GameVersion GameVersion => InitialEventsBatch.CurrentVersion;

    public GameRunningEvent(
        GameId gameId,
        IGameEventsBatch initialEventsBatch)
    {
        GameId = gameId;

        ArgumentNullException.ThrowIfNull(initialEventsBatch);
        InitialEventsBatch = initialEventsBatch;
    }
}
