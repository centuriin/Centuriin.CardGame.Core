using Centuriin.CardGame.Core.Engine.ECS.Systems;

namespace Centuriin.CardGame.Core.Engine.Loading.Configuration.Pipeline;

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