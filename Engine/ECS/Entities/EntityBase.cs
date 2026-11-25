using Centuriin.CardGame.Core.Engine.ECS.Components;

namespace Centuriin.CardGame.Core.Engine.ECS.Entities;

/// <summary>
/// Provides an entity identifier and components indexed by their concrete types.
/// </summary>
public abstract class EntityBase
{
    private readonly ComponentsCollection _components = new();

    /// <summary>
    /// Gets the identifier of this entity within its game.
    /// </summary>
    public EntityId Id { get; }

    /// <summary>
    /// Initializes a new <see cref="EntityBase"/> instance.
    /// </summary>
    /// <param name="id">
    /// The entity identifier.
    /// </param>
    protected EntityBase(EntityId id)
    {
        Id = id;
    }

    /// <summary>
    /// Initializes a new <see cref="EntityBase"/> instance.
    /// </summary>
    /// <param name="id">
    /// The entity identifier.
    /// </param>
    /// <param name="components">
    /// The components assigned to the entity or template.
    /// </param>
    protected EntityBase(
        EntityId id,
        IEnumerable<ComponentBase> components)
        : this(id)
    {
        ArgumentNullException.ThrowIfNull(components);
        _components = new ComponentsCollection(components);
    }

    /// <inheritdoc cref="ComponentsCollection.Get{T}"/>
    public T Get<T>() where T : ComponentBase => _components.Get<T>();

    /// <inheritdoc cref="ComponentsCollection.Has{T}"/>
    public bool Has<T>() where T : ComponentBase => _components.Has<T>();

    /// <inheritdoc cref="ComponentsCollection.Set"/>
    internal void Set(params IEnumerable<ComponentBase> components)
    {
        ArgumentNullException.ThrowIfNull(components);
        _components.Set(components);
    }

    /// <inheritdoc cref="ComponentsCollection.Remove{T}"/>
    internal void Remove<T>() where T : ComponentBase => _components.Remove<T>();
}
