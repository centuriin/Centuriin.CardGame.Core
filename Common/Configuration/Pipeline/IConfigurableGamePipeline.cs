using Centuriin.CardGame.Core.Common.ECS.Systems;

namespace Centuriin.CardGame.Core.Common.Configuration.Pipeline;

public interface IConfigurableGamePipeline
{
    public IConfigurableGamePipeline UseDefaultProfile();

    public IConfigurableSystem Add<TSystem>()
        where TSystem : ISystem;

    public IConfigurableSystem AddAfter<TBellowSystem, TSystem>()
        where TBellowSystem : ISystem
        where TSystem : ISystem;

    public IConfigurableSystem AddBefore<TFollowSystem, TSystem>()
        where TFollowSystem : ISystem
        where TSystem : ISystem;

    public IConfigurableSystem Replace<TReplaceableSystem, TSystem>()
        where TReplaceableSystem : ISystem
        where TSystem : ISystem;
}