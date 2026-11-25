using System.Collections;

using Centuriin.CardGame.Core.Engine.Events.Domain;
using Centuriin.CardGame.Core.Engine.Root.State;

namespace Centuriin.CardGame.Core.Engine.Events;

/// <summary>
/// Stores an ordered list of domain events and its game version.
/// </summary>
internal sealed class GameEventsBatch : IGameEventsBatch
{
    /// <inheritdoc/>
    public GameVersion GameVersion { get; }

    /// <inheritdoc/>
    public IReadOnlyList<IGameEvent> Events { get; }

    /// <summary>
    /// Initializes a new <see cref="GameEventsBatch"/> instance.
    /// </summary>
    /// <param name="currentGameVersion">
    /// The version assigned to the event batch.
    /// </param>
    /// <param name="events">
    /// The domain events in application order.
    /// </param>
    public GameEventsBatch(GameVersion currentGameVersion, IReadOnlyList<IGameEvent> events)
    {
        GameVersion = currentGameVersion;

        ArgumentNullException.ThrowIfNull(events);
        Events = events;
    }

    /// <inheritdoc/>
    public IEnumerator<IGameEvent> GetEnumerator() => Events.GetEnumerator();

    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
