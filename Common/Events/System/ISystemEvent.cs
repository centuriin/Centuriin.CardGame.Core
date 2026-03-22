using Centuriin.Centuriin.Core.Common;

namespace Centuriin.CardGame.Core.Common.Events.System;

public interface ISystemEvent
{
    public GameId GameId { get; }
}