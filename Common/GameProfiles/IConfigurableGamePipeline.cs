using Centuriin.CardGame.Core.Common.Events;
using Centuriin.CardGame.Core.Common.Events.Dispatching;
using Centuriin.CardGame.Core.Common.Systems;

namespace Centuriin.CardGame.Core.Common.GameProfiles;

public interface IConfigurableGamePipeline
{
    public IConfigurableGamePipeline UseDefaultProfile();

    public IConfigurableGamePipeline Add<TSystem, TEvent>()
        where TSystem : SystemBase, ISubscriber<TEvent>
        where TEvent : IGameEvent;

    public IConfigurableGamePipeline AddAfter<TBellowSystem, TSystem, TEvent>()
        where TBellowSystem : SystemBase
        where TSystem : SystemBase, ISubscriber<TEvent>
        where TEvent : IGameEvent;

    public IConfigurableGamePipeline AddBefore<TFollowSystem, TSystem, TEvent>()
        where TFollowSystem : SystemBase
        where TSystem : SystemBase, ISubscriber<TEvent>
        where TEvent : IGameEvent;

    public IConfigurableGamePipeline Replace<TReplaceableSystem, TSystem, TEvent>()
        where TReplaceableSystem : SystemBase
        where TSystem : SystemBase, ISubscriber<TEvent>
        where TEvent : IGameEvent;
}