namespace Centuriin.CardGame.Core.Common.Events.Dispatching;

public interface IGameEventBusWriter
{
    void Write(IGameEvent @event);
}