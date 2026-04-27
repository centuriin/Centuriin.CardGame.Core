namespace Centuriin.CardGame.Core.Common.Events.Dispatching;

public interface IGameEventBus
{
    public bool TryRead(out IGameEvent @event);

    public void Write(IGameEvent @event);
}