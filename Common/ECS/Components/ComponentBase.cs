namespace Centuriin.CardGame.Core.Common.ECS.Components;

/// <summary>
/// Base class for components.
/// </summary>
public abstract record class ComponentBase
{
    /// <inheritdoc/>
    public ComponentBase Copy() => this with { };
}