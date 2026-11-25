using Centuriin.CardGame.Core.Engine.ECS.Templates;

namespace Centuriin.CardGame.Core.Engine.ECS.Entities.Factories;

/// <summary>
/// Creates entities with sequential identifiers from a collection of templates.
/// </summary>
/// <typeparam name="TEntity">
/// The entity type.
/// </typeparam>
/// <typeparam name="TTemplate">
/// The entity template type.
/// </typeparam>
public abstract class EntityFactoryBase<TEntity, TTemplate> :
    IEntityFactory<TEntity, TTemplate>
    where TEntity : EntityBase
    where TTemplate : TemplateBase
{
    private readonly IEntityIdProvider _entityIdProvider;

    /// <summary>
    /// Initializes a new <see cref="EntityFactoryBase{TEntity, TTemplate}"/> instance.
    /// </summary>
    /// <param name="entityIdProvider">
    /// The provider used to allocate entity identifiers.
    /// </param>
    protected EntityFactoryBase(IEntityIdProvider entityIdProvider)
    {
        ArgumentNullException.ThrowIfNull(entityIdProvider);
        _entityIdProvider = entityIdProvider;
    }

    /// <inheritdoc/>
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

    /// <summary>
    /// Creates an entity with the supplied identifier and template components.
    /// </summary>
    /// <param name="id">
    /// The entity identifier.
    /// </param>
    /// <param name="template">
    /// The template containing the initial components.
    /// </param>
    /// <returns>
    /// The created instance.
    /// </returns>
    protected abstract TEntity CreateEntity(EntityId id, TTemplate template);
}