using Centuriin.CardGame.Core.Common.Events.Domain;
using Centuriin.CardGame.Core.Common.World;

namespace Centuriin.CardGame.Core.Common.Events.Application;

public sealed class NewGameEventsBatchCreatedEvent : IApplicationEvent
{
    public GameId GameId { get; }

    public GameVersion GameVersion { get; }

    public IGameEventsBatch Batch { get; }

    public NewGameEventsBatchCreatedEvent(
        GameId gameId,
        GameVersion gameVersion,
        IGameEventsBatch batch)
    {
        GameId = gameId;
        GameVersion = gameVersion;

        ArgumentNullException.ThrowIfNull(batch);
        Batch = batch;
    }
}
