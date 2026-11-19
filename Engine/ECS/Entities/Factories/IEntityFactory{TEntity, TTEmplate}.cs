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
    public IReadOnlyList<EditableEntity<TEntity>> Create(
        params IReadOnlyCollection<TTemplate> templates);
}
