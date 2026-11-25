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

    /// <inheritdoc cref="EntityBase.Id"/>
    public EntityId Id => _entity.Id;

    /// <summary>
    /// Wrapped entity.
    /// </summary>
    public TEntity Entity => _entity;

    /// <summary>
    /// Initializes a new <see cref="EditableEntity{TEntity}"/> instance.
    /// </summary>
    /// <param name="entity">
    /// The entity to wrap and edit.
    /// </param>
    internal EditableEntity(TEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        _entity = entity;
    }

    /// <inheritdoc cref="EntityBase.Get{T}"/>
    public T Get<T>() where T : ComponentBase => _entity.Get<T>();

    /// <inheritdoc cref="EntityBase.Has{T}"/>
    public bool Has<T>() where T : ComponentBase => _entity.Has<T>();

    /// <inheritdoc cref="ComponentsCollection.Set"/>
    /// <returns>
    /// This wrapper for further component edits.
    /// </returns>
    public EditableEntity<TEntity> Set(params IEnumerable<ComponentBase> components)
    {
        ArgumentNullException.ThrowIfNull(components);

        _entity.Set(components);

        return this;
    }

    /// <inheritdoc cref="ComponentsCollection.Remove{T}"/>
    /// <returns>
    /// This wrapper for further component edits.
    /// </returns>
    public EditableEntity<TEntity> Remove<T>() where T : ComponentBase
    {
        _entity.Remove<T>();

        return this;
    }

    /// <summary>
    /// Returns the entity referenced by an editable wrapper.
    /// </summary>
    /// <param name="c">
    /// The editable wrapper whose underlying entity is returned.
    /// </param>
    /// <returns>
    /// The resulting value.
    /// </returns>
    public static implicit operator TEntity(EditableEntity<TEntity> c) => c._entity;
}
