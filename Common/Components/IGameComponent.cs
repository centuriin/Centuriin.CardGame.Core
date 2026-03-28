namespace Centuriin.CardGame.Core.Common.Components;

/// <summary>
/// Game component.
/// </summary>
public interface IGameComponent
{
    /// <summary>
    /// Creates a clone.
    /// </summary>
    /// <returns>
    /// Cloned component <see cref="IGameComponent"/>.
    /// </returns>
    public IGameComponent Copy();
}