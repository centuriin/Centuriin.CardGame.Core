using Centuriin.CardGame.Core.Engine.ECS.Components;
using Centuriin.CardGame.Core.Engine.ECS.Entities;

namespace Centuriin.CardGame.Core.Engine;

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
        Func<TComponent, bool> predicate)
        where TComponent : ComponentBase
    {
        ArgumentNullException.ThrowIfNull(entities);
        ArgumentNullException.ThrowIfNull(predicate);

        return entities.Where(x => x.Has<TComponent>() && predicate(x.Get<TComponent>()));
    }
}
