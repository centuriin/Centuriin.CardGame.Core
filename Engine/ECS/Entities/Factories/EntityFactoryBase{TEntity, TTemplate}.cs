using Centuriin.CardGame.Core.Engine.ECS.Templates;

namespace Centuriin.CardGame.Core.Engine.ECS.Entities.Factories;

public abstract class EntityFactoryBase<TEntity, TTemplate> :
    IEntityFactory<TEntity, TTemplate>
    where TEntity : EntityBase
    where TTemplate : TemplateBase
{
    private readonly IEntityIdProvider _entityIdProvider;

    protected EntityFactoryBase(IEntityIdProvider entityIdProvider)
    {
        ArgumentNullException.ThrowIfNull(entityIdProvider);
        _entityIdProvider = entityIdProvider;
    }

    public IReadOnlyList<EditableEntity<TEntity>> Create(
        params IReadOnlyCollection<TTemplate> templates)
    {
        ArgumentNullException.ThrowIfNull(templates);

        var entities = new List<EditableEntity<TEntity>>(templates.Count);

        foreach(var template in templates)
        {
            entities.Add(new(CreateEntity(_entityIdProvider.GetNextId(), template)));
        }

        return entities;
    }

    protected abstract TEntity CreateEntity(EntityId id, TTemplate template);
}