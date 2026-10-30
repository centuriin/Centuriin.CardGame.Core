using Centuriin.CardGame.Core.Common.Events.Domain;

namespace Centuriin.CardGame.Core.Common.Events.Application;

public interface IGameEventApplier
{
    public void Apply<TEvent>(TEvent @event)
        where TEvent : IGameEvent;
}