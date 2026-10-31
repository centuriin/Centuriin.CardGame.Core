using Centuriin.CardGame.Core.Common.Events.Domain;

namespace Centuriin.CardGame.Core.Common.Events;

public interface IGameEventApplier
{
    public void Apply(IGameEvent @event);
}