using Centuriin.CardGame.Core.Engine.Root.State;

namespace Centuriin.CardGame.Core.Engine.Events.Application;

public sealed record class GameAbortedEvent(
    GameId GameId,
    GameVersion GameVersion) : IGameApplicationEvent;