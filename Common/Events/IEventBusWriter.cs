namespace Centuriin.CardGame.Core.Common.Events;

public interface IEventBusWriter
{
    public void Write(IGameEvent @event);
}
