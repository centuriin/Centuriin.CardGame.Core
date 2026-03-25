using Centuriin.CardGame.Core.Common.Entities.Cards;
using Centuriin.Centuriin.Core.Common;

namespace Centuriin.CardGame.Core.Common.Events.System;

public sealed record class CardInstanceCreated(
    GameId GameId,
    CardId CardId,
    TemplateId TemplateId) : ISystemEvent;