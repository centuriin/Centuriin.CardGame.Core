using Centuriin.CardGame.Core.Common.Root.State;

namespace Centuriin.CardGame.Core.Common;

public sealed record class GameDescription(GameId GameId, GameVersion GameVersion);