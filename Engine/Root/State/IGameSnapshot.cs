using Centuriin.CardGame.Core.Engine.ECS.Entities;

namespace Centuriin.CardGame.Core.Engine.Root.State;

/// <summary>
/// Exposes stored game metadata and entities for loading.
/// </summary>
public interface IGameSnapshot
{
    /// <summary>
    /// Gets the identifier of the associated game.
    /// </summary>
    public GameId GameId { get; }

    /// <summary>
    /// Gets the identifier of the game type stored in the snapshot.
    /// </summary>
    public GameTypeId TypeId { get; }

    /// <summary>
    /// Gets the game status stored in the snapshot.
    /// </summary>
    public GameStatus GameStatus { get; }

    /// <summary>
    /// Gets the current game or snapshot version.
    /// </summary>
    public GameVersion Version { get; }

    /// <summary>
    /// Gets the last allocated entity identifier.
    /// </summary>
    public EntityId LastEntityId { get; }

    /// <summary>
    /// Gets the entities available in this state or context.
    /// </summary>
    public IReadOnlyEntitiesCollection Entities { get; }
}