using Centuriin.CardGame.Core.Engine.ECS.Components;

namespace Centuriin.CardGame.Core.Engine.ECS.Entities;

/// <summary>
/// Base class for entity.
/// </summary>
public abstract class EntityBase
{
    private readonly ComponentsCollection _components = new();

    /// <summary>
    /// Instance id.
    /// </summary>
    public EntityId Id { get; }

    protected EntityBase(EntityId id)
    {
        Id = id;
    }

    protected EntityBase(
        EntityId id,
        IEnumerable<ComponentBase> components)
        : this(id)
    {
        ArgumentNullException.ThrowIfNull(components);
        _components = new ComponentsCollection(components);
    }

    /// <inheritdoc/>
    public T Get<T>() where T : ComponentBase => _components.Get<T>();

    /// <inheritdoc/>
    public bool Has<T>() where T : ComponentBase => _components.Has<T>();

    /// <summary>
    /// Sets or resets components.
    /// </summary>
    /// <param name="components">
    /// Components.
    /// </param>
    internal void Set(params IEnumerable<ComponentBase> components)
    {
        ArgumentNullException.ThrowIfNull(components);
        _components.Set(components);
    }

    /// <summary>
    /// Removes component from entity.
    /// </summary>
    /// <typeparam name="T">
    /// Component type.
    /// </typeparam>
    internal void Remove<T>() where T : ComponentBase => _components.Remove<T>();
}
