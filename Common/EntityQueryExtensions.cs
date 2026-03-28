using Centuriin.CardGame.Core.Common.Components;
using Centuriin.CardGame.Core.Common.Entities;

namespace Centuriin.CardGame.Core.Common;

public static class EntityQueryExtensions
{
    public static IEnumerable<EntityBase> WithComponent<TComponent>(
        this IEnumerable<EntityBase> entities)
        where TComponent : ComponentBase
    {
        ArgumentNullException.ThrowIfNull(entities);

        return entities.Where(x => x.Has<TComponent>());
    }

    public static IEnumerable<EntityBase> WithComponent<TComponent>(
        this IEnumerable<EntityBase> entities,
        Func<TComponent, bool> preicate)
        where TComponent : ComponentBase
    {
        ArgumentNullException.ThrowIfNull(entities);
        ArgumentNullException.ThrowIfNull(preicate);

        return entities.Where(x => x.Has<TComponent>() && preicate(x.Get<TComponent>()));
    }

    public static IEnumerable<TEntity> As<TEntity>(
        this IEnumerable<EntityBase> entities)
        where TEntity : EntityBase
    {
        ArgumentNullException.ThrowIfNull(entities);

        return entities.Select(x => (TEntity)x);
    }
}
