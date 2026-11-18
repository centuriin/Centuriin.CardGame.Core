using Centuriin.CardGame.Core.Engine.ECS.Components;

namespace Centuriin.CardGame.Core.Engine.ECS.Entities;

/// <summary>
/// Mutable entity wrapper.
/// </summary>
/// <typeparam name="TEntity">
/// Entity type.
/// </typeparam>
public readonly struct EditableEntity<TEntity>
    where TEntity : EntityBase
{
    private readonly TEntity _entity;

    /// <summary>
    /// Instance id.
    /// </summary>
    public EntityId Id => _entity.Id;

    public EditableEntity(TEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        _entity = entity;
    }

    /// <inheritdoc/>
    public T Get<T>() where T : ComponentBase => _entity.Get<T>();

    /// <inheritdoc/>
    public bool Has<T>() where T : ComponentBase => _entity.Has<T>();

    /// <summary>
    /// Sets or resets components.
    /// </summary>
    /// <param name="components">
    /// Components.
    /// </param>
    public void Set(params IEnumerable<ComponentBase> components) => _entity.Set(components);

    /// <summary>
    /// Removes component from entity.
    /// </summary>
    /// <typeparam name="T">
    /// Component type.
    /// </typeparam>
    public void Remove<T>() where T : ComponentBase => _entity.Remove<T>();

    public static implicit operator TEntity(EditableEntity<TEntity> c) => c._entity;
}
