using Centuriin.CardGame.Core.Common.Events.Domain;

namespace Centuriin.CardGame.Core.Common.Events.Dispatching;

public interface IGameEventBusWriter
{
    public void Write(IGameEvent @event);
}