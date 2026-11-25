using Centuriin.CardGame.Core.Engine.ECS.Templates;

namespace Centuriin.CardGame.Core.Engine.ECS.Entities.Factories;

/// <summary>
/// Entity factory.
/// </summary>
/// <typeparam name="TEntity">
/// Entity type.
/// </typeparam>
/// <typeparam name="TTemplate">
/// Template type.
/// </typeparam>
public interface IEntityFactory<TEntity, TTemplate>
    where TEntity : EntityBase
    where TTemplate : TemplateBase
{
    /// <summary>
    /// Creates an editable entity for each supplied template.
    /// </summary>
    /// <param name="templates">
    /// The templates used to create entities.
    /// </param>
    /// <returns>
    /// The created entities in template order.
    /// </returns>
    public IReadOnlyList<EditableEntity<TEntity>> Create(
        params IReadOnlyCollection<TTemplate> templates);
}
