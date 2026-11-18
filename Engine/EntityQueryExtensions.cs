using Centuriin.CardGame.Core.Engine.ECS.Components;
using Centuriin.CardGame.Core.Engine.ECS.Entities;

namespace Centuriin.CardGame.Core.Engine;

public static class EntityQueryExtensions
{
    public static IEnumerable<TEntity> WithComponent<TEntity, TComponent>(
        this IEnumerable<TEntity> entities)
        where TEntity : EntityBase
        where TComponent : ComponentBase
    {
        ArgumentNullException.ThrowIfNull(entities);

        return entities.Where(x => x.Has<TComponent>());
    }

    public static IEnumerable<EditableEntity<TEntity>> WithComponent<TEntity, TComponent>(
        this IEnumerable<EditableEntity<TEntity>> entities)
        where TEntity : EntityBase
        where TComponent : ComponentBase
    {
        ArgumentNullException.ThrowIfNull(entities);

        return entities.Where(x => x.Has<TComponent>());
    }

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
