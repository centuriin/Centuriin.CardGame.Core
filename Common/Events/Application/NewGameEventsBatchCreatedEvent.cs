using Centuriin.CardGame.Core.Common.Events.Domain;
using Centuriin.CardGame.Core.Common.World;

namespace Centuriin.CardGame.Core.Common.Events.Application;

public sealed class NewGameEventsBatchCreatedEvent : IGameApplicationEvent
{
    public GameId GameId { get; }

    public IGameEventsBatch Batch { get; }

    public GameVersion GameVersion => Batch.CurrentVersion;

    public NewGameEventsBatchCreatedEvent(
        GameId gameId,
        IGameEventsBatch batch)
    {
        GameId = gameId;

        ArgumentNullException.ThrowIfNull(batch);
        Batch = batch;
    }
}
