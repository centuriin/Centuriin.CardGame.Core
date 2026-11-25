using Centuriin.CardGame.Core.Engine.ECS.Components;
using Centuriin.CardGame.Core.Engine.ECS.Entities;

namespace Centuriin.CardGame.Core.Engine;

/// <summary>
/// Filters entity sequences by component presence and values.
/// </summary>
public static class EntityQueryExtensions
{
    /// <summary>
    /// Filters entities by component presence and, when supplied, a component predicate.
    /// </summary>
    /// <typeparam name="TEntity">
    /// The entity type.
    /// </typeparam>
    /// <typeparam name="TComponent">
    /// The component type used by the query or operation.
    /// </typeparam>
    /// <param name="entities">
    /// The entities to process.
    /// </param>
    /// <returns>
    /// The entities whose components satisfy the filter.
    /// </returns>
    public static IEnumerable<TEntity> WithComponent<TEntity, TComponent>(
        this IEnumerable<TEntity> entities)
        where TEntity : EntityBase
        where TComponent : ComponentBase
    {
        ArgumentNullException.ThrowIfNull(entities);

        return entities.Where(x => x.Has<TComponent>());
    }

    /// <summary>
    /// Filters entities by component presence and, when supplied, a component predicate.
    /// </summary>
    /// <typeparam name="TEntity">
    /// The entity type.
    /// </typeparam>
    /// <typeparam name="TComponent">
    /// The component type used by the query or operation.
    /// </typeparam>
    /// <param name="entities">
    /// The entities to process.
    /// </param>
    /// <returns>
    /// The entities whose components satisfy the filter.
    /// </returns>
    public static IEnumerable<EditableEntity<TEntity>> WithComponent<TEntity, TComponent>(
        this IEnumerable<EditableEntity<TEntity>> entities)
        where TEntity : EntityBase
        where TComponent : ComponentBase
    {
        ArgumentNullException.ThrowIfNull(entities);

        return entities.Where(x => x.Has<TComponent>());
    }

    /// <summary>
    /// Filters entities by component presence and, when supplied, a component predicate.
    /// </summary>
    /// <typeparam name="TEntity">
    /// The entity type.
    /// </typeparam>
    /// <typeparam name="TComponent">
    /// The component type used by the query or operation.
    /// </typeparam>
    /// <param name="entities">
    /// The entities to process.
    /// </param>
    /// <param name="predicate">
    /// The condition that the component must satisfy.
    /// </param>
    /// <returns>
    /// The entities whose components satisfy the filter.
    /// </returns>
    public static IEnumerable<TEntity> WithComponent<TEntity, TComponent>(
        this IEnumerable<TEntity> entities,
        Func<TComponent, bool> predicate)
        where TEntity : EntityBase
        where TComponent : ComponentBase
    {
        ArgumentNullException.ThrowIfNull(entities);
        ArgumentNullException.ThrowIfNull(predicate);

        return entities.Where(x => x.Has<TComponent>() && predicate(x.Get<TComponent>()));
    }

    /// <summary>
    /// Filters entities by component presence and, when supplied, a component predicate.
    /// </summary>
    /// <typeparam name="TEntity">
    /// The entity type.
    /// </typeparam>
    /// <typeparam name="TComponent">
    /// The component type used by the query or operation.
    /// </typeparam>
    /// <param name="entities">
    /// The entities to process.
    /// </param>
    /// <param name="predicate">
    /// The condition that the component must satisfy.
    /// </param>
    /// <returns>
    /// The entities whose components satisfy the filter.
    /// </returns>
    public static IEnumerable<EditableEntity<TEntity>> WithComponent<TEntity, TComponent>(
        this IEnumerable<EditableEntity<TEntity>> entities,
        Func<TComponent, bool> predicate)
        where TEntity : EntityBase
        where TComponent : ComponentBase
    {
        ArgumentNullException.ThrowIfNull(entities);
        ArgumentNullException.ThrowIfNull(predicate);

        return entities.Where(x => x.Has<TComponent>() && predicate(x.Get<TComponent>()));
    }
}
