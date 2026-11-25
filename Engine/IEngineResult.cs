using Centuriin.CardGame.Core.Engine.Root.State;

namespace Centuriin.CardGame.Core.Engine;

/// <summary>
/// Exposes the game version associated with an engine operation.
/// </summary>
public interface IEngineResult
{
    /// <summary>
    /// Gets the game version associated with this value.
    /// </summary>
    public GameVersion GameVersion { get; }
}
