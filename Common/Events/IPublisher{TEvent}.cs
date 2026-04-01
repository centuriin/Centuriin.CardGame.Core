using System;
using System.Collections.Generic;
using System.Text;

namespace Centuriin.CardGame.Core.Common.Events;

public interface IPublisher<TEvent>
    where TEvent : IGameEvent
{
}
