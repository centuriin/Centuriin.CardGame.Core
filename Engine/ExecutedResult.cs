using Centuriin.CardGame.Core.Engine.Events.Domain;
using Centuriin.CardGame.Core.Engine.Root.State;

namespace Centuriin.CardGame.Core.Engine;

/// <summary>
/// Contains the events produced and applied by a successful authoritative operation.
/// </summary>
public sealed class ExecutedResult : IEngineResult
{
    /// <inheritdoc/>
    public GameVersion GameVersion { get; }

    /// <summary>
    /// Gets the ordered batch of domain events.
    /// </summary>
    public IGameEventsBatch Batch { get; }

    /// <summary>
    /// Initializes a new <see cref="ExecutedResult"/> instance.
    /// </summary>
    /// <param name="batch">
    /// The ordered batch of domain events.
    /// </param>
    public ExecutedResult(IGameEventsBatch batch)
    {
        ArgumentNullException.ThrowIfNull(batch);
        Batch = batch;

        GameVersion = batch.GameVersion;
    }
}
