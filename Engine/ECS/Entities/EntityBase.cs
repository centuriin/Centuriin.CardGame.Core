using System.Collections;

using Centuriin.CardGame.Core.Engine.ECS.Components;

namespace Centuriin.CardGame.Core.Engine.ECS.Entities;

/// <summary>
/// Base class for entity.
/// </summary>
public abstract class EntityBase : 
    IComponentsCollection,
    IReadOnlyEntity
{
    private readonly ComponentsCollection _components = [];

    /// <summary>
    /// Instance id.
    /// </summary>
    public EntityId Id { get; }

    /// <inheritdoc/>
    public bool IsEmpty { get; }

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
    public void Add(params IEnumerable<ComponentBase> components)
    {
        ArgumentNullException.ThrowIfNull(components);
        _components.Add(components);
    }

    /// <inheritdoc/>
    public void Replace<T>(T replacement) where T : ComponentBase
    {
        ArgumentNullException.ThrowIfNull(replacement);
        _components.Replace(replacement);
    }

    /// <inheritdoc/>
    public void Remove<T>() where T : ComponentBase => _components.Remove<T>();

    /// <inheritdoc/>
    public T Get<T>() where T : ComponentBase => _components.Get<T>();

    /// <inheritdoc/>
    public bool Has<T>() where T : ComponentBase => _components.Has<T>();

    /// <inheritdoc/>
    public IEnumerator<ComponentBase> GetEnumerator() => _components.GetEnumerator();

    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
