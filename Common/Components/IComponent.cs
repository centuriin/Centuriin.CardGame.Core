namespace Centuriin.CardGame.Core.Common.Components;

/// <summary>
/// Card component.
/// </summary>
public interface IComponent
{
    /// <summary>
    /// Creates a clone.
    /// </summary>
    /// <returns>
    /// Cloned component <see cref="IComponent"/>.
    /// </returns>
    public IComponent Copy();
}