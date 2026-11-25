using Centuriin.CardGame.Core.Engine.Root.State;

namespace Centuriin.CardGame.Core.Engine.Events.Domain;

/// <summary>
/// Exposes the ordered events associated with one game version.
/// </summary>
public interface IGameEventsBatch : IEnumerable<IGameEvent>
{
    /// <summary>
    /// Gets the game version associated with this value.
    /// </summary>
    public GameVersion GameVersion { get; }

    /// <summary>
    /// Gets the domain events in application order.
    /// </summary>
    public IReadOnlyList<IGameEvent> Events { get; }
}