using Centuriin.CardGame.Core.Common.ECS.Systems;

namespace Centuriin.CardGame.Core.Common.Configuration.Pipeline;

public interface IConfigurableGamePipeline
{
    public IConfigurableGamePipeline UseDefaultProfile();

    public IConfigurableSystem Add<TSystem>()
        where TSystem : SystemBase;

    public IConfigurableGamePipeline AddAfter<TBellowSystem, TSystem>()
        where TBellowSystem : SystemBase
        where TSystem : SystemBase;

    public IConfigurableGamePipeline AddBefore<TFollowSystem, TSystem>()
        where TFollowSystem : SystemBase
        where TSystem : SystemBase;

    public IConfigurableGamePipeline Replace<TReplaceableSystem, TSystem>()
        where TReplaceableSystem : SystemBase
        where TSystem : SystemBase;
}