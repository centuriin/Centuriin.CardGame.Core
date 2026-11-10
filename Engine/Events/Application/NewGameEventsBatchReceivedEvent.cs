using Centuriin.CardGame.Core.Engine.Events.Domain;
using Centuriin.CardGame.Core.Engine.Root.State;

namespace Centuriin.CardGame.Core.Engine.Events.Application;

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
