using Centuriin.CardGame.Core.Engine;
using Centuriin.CardGame.Core.Engine.Root.State;

namespace Centuriin.CardGame.Core.Application.Events;

/// <summary>
/// Reports that a replica is ready to receive authoritative game events.
/// </summary>
/// <param name="GameId">
/// The identifier of the associated game.
/// </param>
/// <param name="GameVersion">
/// The version associated with the game operation.
/// </param>
public sealed record class GamePreparedEvent(GameId GameId, GameVersion GameVersion) :
    IGameApplicationEvent;