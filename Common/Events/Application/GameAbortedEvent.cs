using Centuriin.CardGame.Core.Common.Root.State;

namespace Centuriin.CardGame.Core.Common.Events.Application;

public sealed record class GameAbortedEvent(
    GameId GameId,
    GameVersion GameVersion) : IGameApplicationEvent;