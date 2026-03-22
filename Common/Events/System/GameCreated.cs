using Centuriin.Centuriin.Core.Common;

namespace Centuriin.CardGame.Core.Common.Events.System;

public sealed record class GameCreated(GameId GameId, GameTypeId TypeId) : ISystemEvent;