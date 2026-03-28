namespace Centuriin.CardGame.Core.Common.Components;

/// <summary>
/// Base class for components.
/// </summary>
public abstract record class ComponentBase
{
    /// <inheritdoc/>
    public ComponentBase Copy() => this with { };
}