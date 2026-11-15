using Centuriin.CardGame.Core.Engine.ECS.Components;

namespace Centuriin.CardGame.Core.Engine.ECS.Entities;

/// <summary>
/// Base class for entity.
/// </summary>
public abstract class EntityBase
{
    /// <summary>
    /// Instance id.
    /// </summary>
    public EntityId Id { get; }

    /// <inheritdoc/>
    public bool IsEmpty { get; }

    protected ComponentsCollection Components { get; } = [];

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
        Components = new ComponentsCollection(components);
    }

    protected EntityBase(
        EntityId id,
        ComponentsCollection components)
        : this(id)
    {
        Components = components;
    }

    /// <inheritdoc/>
    public abstract EntityBase Add(params IEnumerable<ComponentBase> components);

    /// <inheritdoc/>
    public abstract EntityBase Replace(params IEnumerable<ComponentBase> components);

    /// <inheritdoc/>
    public abstract EntityBase Remove<T>() where T : ComponentBase;

    /// <inheritdoc/>
    public T Get<T>() where T : ComponentBase => Components.Get<T>();

    /// <inheritdoc/>
    public bool Has<T>() where T : ComponentBase => Components.Has<T>();
}
