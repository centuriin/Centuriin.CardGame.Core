using Centuriin.CardGame.Core.Common.ECS.Templates;

namespace Centuriin.CardGame.Core.Common.ECS.Components;

/// <summary>
/// Template component.
/// </summary>
/// <param name="TemplateId">
/// Template id.
/// </param>
public sealed record class TemplateComponent(TemplateId TemplateId) : ComponentBase;