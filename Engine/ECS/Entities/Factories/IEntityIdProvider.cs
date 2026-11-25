namespace Centuriin.CardGame.Core.Engine.ECS.Entities.Factories;

/// <summary>
/// Allocates entity identifiers and restores the last allocated value.
/// </summary>
public interface IEntityIdProvider
{
    /// <summary>
    /// Allocates and returns the next entity identifier.
    /// </summary>
    /// <returns>
    /// The newly allocated entity identifier.
    /// </returns>
    public EntityId GetNextId();

    /// <summary>
    /// Sets the last allocated identifier before further entities are created.
    /// </summary>
    /// <param name="id">
    /// The entity identifier.
    /// </param>
    public void SetInitialId(EntityId id);
}
