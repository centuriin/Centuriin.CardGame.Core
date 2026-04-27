namespace Centuriin.CardGame.Core.Common.Events.Dispatching;

public interface IGameEventBusReader
{
    bool TryRead(out IGameEvent @event);
}