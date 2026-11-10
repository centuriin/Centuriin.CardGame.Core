using Centuriin.CardGame.Core.Engine;
using Centuriin.CardGame.Core.Engine.Root.State;

namespace Application.Events;

public sealed record class GameAbortedEvent(
    GameId GameId,
    GameVersion GameVersion) : IGameApplicationEvent;