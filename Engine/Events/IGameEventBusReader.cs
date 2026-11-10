using Centuriin.CardGame.Core.Engine.Events.Domain;

namespace Centuriin.CardGame.Core.Engine.Events;

public interface IGameEventBusReader
{
    public bool TryRead(out IGameEvent @event);
}