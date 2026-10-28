using Centuriin.CardGame.Core.Common.World;

namespace Centuriin.CardGame.Core.Common.Events.Application;

public sealed record class GameRestoredEvent(GameId GameId, GameVersion GameVersion) : IGameApplicationEvent;