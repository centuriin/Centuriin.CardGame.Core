using Centuriin.CardGame.Core.Engine.ECS.Templates;

namespace Centuriin.CardGame.Core.Engine.ECS.Components;

/// <summary>
/// Template component.
/// </summary>
/// <param name="TemplateId">
/// Template id.
/// </param>
public sealed record class TemplateComponent(TemplateId TemplateId) : ComponentBase;