using Centuriin.CardGame.Core.Engine.Root.State;

namespace Centuriin.CardGame.Core.Engine.Events.Application;

public sealed record class GamePreparedEvent(GameId GameId, GameVersion GameVersion) :
    IGameApplicationEvent;