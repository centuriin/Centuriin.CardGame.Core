using Centuriin.CardGame.Core.Common.ECS.Systems;

namespace Centuriin.CardGame.Core.Common.Configuration.Pipeline;

public interface IConfigurableGamePipeline
{
    public IConfigurableGamePipeline UseDefaultProfile();

    public IConfigurableSystem Add<TSystem>()
        where TSystem : SystemBase;

    public IConfigurableSystem AddAfter<TBellowSystem, TSystem>()
        where TBellowSystem : SystemBase
        where TSystem : SystemBase;

    public IConfigurableSystem AddBefore<TFollowSystem, TSystem>()
        where TFollowSystem : SystemBase
        where TSystem : SystemBase;

    public IConfigurableSystem Replace<TReplaceableSystem, TSystem>()
        where TReplaceableSystem : SystemBase
        where TSystem : SystemBase;
}