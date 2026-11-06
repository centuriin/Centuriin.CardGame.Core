using Centuriin.CardGame.Core.Common.Root.State;

namespace Centuriin.CardGame.Core.Common.Events.Application;

public sealed record class GamePreparedEvent(GameId GameId, GameVersion GameVersion) :
    IGameApplicationEvent;