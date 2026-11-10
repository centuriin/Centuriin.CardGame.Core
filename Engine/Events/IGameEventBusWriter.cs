using Centuriin.CardGame.Core.Engine.Events.Domain;

namespace Centuriin.CardGame.Core.Engine.Events;

public interface IGameEventBusWriter
{
    public void Write(IGameEvent @event);
}