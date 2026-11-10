using Centuriin.CardGame.Core.Engine.Events.Domain;

namespace Centuriin.CardGame.Core.Engine.Events;

public interface IGameEventApplier
{
    public void Apply(IGameEvent @event);
}