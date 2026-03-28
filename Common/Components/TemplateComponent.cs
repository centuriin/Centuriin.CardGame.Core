namespace Centuriin.CardGame.Core.Common.Components;

/// <summary>
/// Template component.
/// </summary>
/// <param name="TemplateId">
/// Template id.
/// </param>
public sealed record class TemplateComponent(TemplateId TemplateId) : ComponentBase;