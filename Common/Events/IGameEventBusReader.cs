using Centuriin.CardGame.Core.Common.Events.Domain;

namespace Centuriin.CardGame.Core.Common.Events;

public interface IGameEventBusReader
{
    public bool TryRead(out IGameEvent @event);
}