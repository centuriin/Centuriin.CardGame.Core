using Centuriin.CardGame.Core.Engine;
using Centuriin.CardGame.Core.Engine.Events.Domain;
using Centuriin.CardGame.Core.Engine.Root.State;

namespace Centuriin.CardGame.Core.Application.Events;

/// <summary>
/// Reports that an authoritative game has started and exposes its initial events.
/// </summary>
public sealed class GameRunningEvent : IGameApplicationEvent
{
    /// <inheritdoc/>
    public GameId GameId { get; }

    /// <summary>
    /// Gets the events produced and applied during game startup.
    /// </summary>
    public IGameEventsBatch InitialEventsBatch { get; }

    /// <inheritdoc/>
    public GameVersion GameVersion => InitialEventsBatch.GameVersion;

    /// <summary>
    /// Initializes a new <see cref="GameRunningEvent"/> instance.
    /// </summary>
    /// <param name="gameId">
    /// The identifier of the game.
    /// </param>
    /// <param name="initialEventsBatch">
    /// The events produced during game startup.
    /// </param>
    public GameRunningEvent(
        GameId gameId,
        IGameEventsBatch initialEventsBatch)
    {
        GameId = gameId;

        ArgumentNullException.ThrowIfNull(initialEventsBatch);
        InitialEventsBatch = initialEventsBatch;
    }
}
