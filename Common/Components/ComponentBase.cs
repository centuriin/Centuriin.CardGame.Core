namespace Centuriin.CardGame.Core.Common.Components;

/// <summary>
/// Base class for components.
/// </summary>
public abstract record class ComponentBase : IComponent
{
    /// <inheritdoc/>
    public IComponent Copy() => this with { };
}