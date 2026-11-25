using Centuriin.CardGame.Core.Engine.ECS.Entities;
using Centuriin.CardGame.Core.Engine.Root.State;

namespace Centuriin.CardGame.Core.Engine.Loading;

/// <summary>
/// Loads entities into mutable game state.
/// </summary>
public interface IEntityLoader
{
    /// <summary>
    /// Adds the supported source entities to the target game state.
    /// </summary>
    /// <param name="entitiesCollection">
    /// The source entity collection.
    /// </param>
    /// <param name="gameState">
    /// The game state used by the operation.
    /// </param>
    public void Load(
        IReadOnlyEntitiesCollection entitiesCollection,
        IGameState gameState);
}