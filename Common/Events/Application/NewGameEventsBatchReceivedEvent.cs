using Centuriin.CardGame.Core.Common.Events.Domain;
using Centuriin.CardGame.Core.Common.Root.State;

namespace Centuriin.CardGame.Core.Common.Events.Application;

public sealed class NewGameEventsBatchReceivedEvent : IGameApplicationEvent
{
    public GameId GameId { get; }

    public GameVersion GameVersion => Batch.CurrentGameVersion;

    public IGameEventsBatch Batch { get; }

    public NewGameEventsBatchReceivedEvent(GameId gameId, IGameEventsBatch batch)
    {
        GameId = gameId;

        ArgumentNullException.ThrowIfNull(batch);
        Batch = batch;
    }
}
