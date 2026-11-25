using Centuriin.CardGame.Core.Engine;
using Centuriin.CardGame.Core.Engine.Root.State;

namespace Centuriin.CardGame.Core.Application.Events;

/// <summary>
/// Identifies the game and version associated with an application notification.
/// </summary>
public interface IGameApplicationEvent
{
    /// <summary>
    /// Gets the identifier of the associated game.
    /// </summary>
    public GameId GameId { get; }

    /// <summary>
    /// Gets the game version associated with this value.
    /// </summary>
    public GameVersion GameVersion { get; }
}