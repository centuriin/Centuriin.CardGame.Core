using Centuriin.CardGame.Core.Engine;
using Centuriin.CardGame.Core.Engine.Events.Domain;
using Centuriin.CardGame.Core.Engine.Root.State;

namespace Centuriin.CardGame.Core.Application.Events;

public sealed class NewGameEventsBatchReceivedEvent : IGameApplicationEvent
{
    public GameId GameId { get; }

    public GameVersion GameVersion => Batch.GameVersion;

    public IGameEventsBatch Batch { get; }

    public NewGameEventsBatchReceivedEvent(GameId gameId, IGameEventsBatch batch)
    {
        GameId = gameId;

        ArgumentNullException.ThrowIfNull(batch);
        Batch = batch;
    }
}
