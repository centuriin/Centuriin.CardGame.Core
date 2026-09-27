using Centuriin.CardGame.Core.Common.Events.Dispatching;
using Centuriin.CardGame.Core.Common.Events.Domain;

namespace Centuriin.CardGame.Core.Common.ECS.Systems;

public interface IDealerSystem :
    ISubscriber<GameStartedEvent>,
    ISubscriber<TurnEndedEvent>
{
}