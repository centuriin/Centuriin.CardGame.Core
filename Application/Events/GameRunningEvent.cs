using Centuriin.CardGame.Core.Engine;
using Centuriin.CardGame.Core.Engine.Events.Domain;
using Centuriin.CardGame.Core.Engine.Root.State;

namespace Application.Events;

public sealed class GameRunningEvent : IGameApplicationEvent
{
    public GameId GameId { get; }

    public IGameEventsBatch InitialEventsBatch { get; }

    public GameVersion GameVersion => InitialEventsBatch.CurrentGameVersion;

    public GameRunningEvent(
        GameId gameId,
        IGameEventsBatch initialEventsBatch)
    {
        GameId = gameId;

        ArgumentNullException.ThrowIfNull(initialEventsBatch);
        InitialEventsBatch = initialEventsBatch;
    }
}
