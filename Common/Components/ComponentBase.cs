namespace Centuriin.CardGame.Core.Common.Components;

/// <summary>
/// Base class for components.
/// </summary>
public abstract record class ComponentBase : IGameComponent
{
    /// <inheritdoc/>
    public IGameComponent Copy() => this with { };
}