using Centuriin.CardGame.Core.Common.World;

namespace Centuriin.CardGame.Core.Common.Events.Application;

public sealed record class GameRunningEvent(
    GameId GameId,
    GameVersion GameVersion) : IApplicationEvent;
