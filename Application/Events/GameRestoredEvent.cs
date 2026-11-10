using Centuriin.CardGame.Core.Engine;
using Centuriin.CardGame.Core.Engine.Root.State;

namespace Application.Events;

public sealed record class GameRestoredEvent(GameId GameId, GameVersion GameVersion) : IGameApplicationEvent;