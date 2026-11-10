using Centuriin.CardGame.Core.Engine.Events.Domain;
using Centuriin.CardGame.Core.Engine.Root.State;

namespace Centuriin.CardGame.Core.Engine;

public sealed class ExecutedResult : IEngineResult
{
    public GameVersion GameVersion { get; }

    public IGameEventsBatch Batch { get; }

    public ExecutedResult(IGameEventsBatch batch)
    {
        ArgumentNullException.ThrowIfNull(batch);
        Batch = batch;

        GameVersion = batch.GameVersion;
    }
}
