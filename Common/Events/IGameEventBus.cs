namespace Centuriin.CardGame.Core.Common.Events;

public interface IGameEventBus : IGameEventBusReader, IGameEventBusWriter
{
    public void Clear();
}