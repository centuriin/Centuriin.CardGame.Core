using Centuriin.CardGame.Core.Engine.Root.State;

namespace Centuriin.CardGame.Core.Engine.Events.Application;

public sealed record class GameRestoredEvent(GameId GameId, GameVersion GameVersion) : IGameApplicationEvent;