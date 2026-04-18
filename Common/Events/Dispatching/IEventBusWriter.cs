namespace Centuriin.CardGame.Core.Common.Events.Dispatching;

public interface IEventBusWriter
{
    public void Write(IGameEvent @event);
}
