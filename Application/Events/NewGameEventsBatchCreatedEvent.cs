using Centuriin.CardGame.Core.Engine;
using Centuriin.CardGame.Core.Engine.Events.Domain;
using Centuriin.CardGame.Core.Engine.Root.State;

namespace Centuriin.CardGame.Core.Application.Events;

/// <summary>
/// Reports a batch produced and applied by the authoritative game.
/// </summary>
public sealed class NewGameEventsBatchCreatedEvent : IGameApplicationEvent
{
    /// <inheritdoc/>
    public GameId GameId { get; }

    /// <inheritdoc/>
    public GameVersion GameVersion => Batch.GameVersion;

    /// <summary>
    /// Gets the ordered batch of domain events.
    /// </summary>
    public IGameEventsBatch Batch { get; }

    /// <summary>
    /// Initializes a new <see cref="NewGameEventsBatchCreatedEvent"/> instance.
    /// </summary>
    /// <param name="gameId">
    /// The identifier of the game.
    /// </param>
    /// <param name="batch">
    /// The ordered batch of domain events.
    /// </param>
    public NewGameEventsBatchCreatedEvent(
        GameId gameId,
        IGameEventsBatch batch)
    {
        GameId = gameId;

        ArgumentNullException.ThrowIfNull(batch);
        Batch = batch;
    }
}
