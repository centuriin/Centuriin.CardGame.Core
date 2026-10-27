using Centuriin.CardGame.Core.Common.ECS.Systems;
using Centuriin.CardGame.Core.Common.Events.Dispatching;
using Centuriin.CardGame.Core.Common.Events.Domain;

namespace Centuriin.CardGame.Core.Common.Configuration.Pipeline;

public sealed class GamePipelineInitializer : IGameConfigurationInitializer
{
    private readonly IDispatcher<IGameEvent> _dispatcher;
    private readonly ISystemFactory _systemFactory;
    private readonly ConfigurableGamePipeline _configurableGamePipeline = new();

    public GamePipelineInitializer(
        IDispatcher<IGameEvent> dispatcher,
        ISystemFactory systemFactory)
    {
        ArgumentNullException.ThrowIfNull(dispatcher);
        _dispatcher = dispatcher;

        ArgumentNullException.ThrowIfNull(systemFactory);
        _systemFactory = systemFactory;
    }

    public void Initialize(IGameProfile profile)
    {
        ArgumentNullException.ThrowIfNull(profile);

        profile.Configure(_configurableGamePipeline);

        var configurableSystems = _configurableGamePipeline.Freeze();

        foreach (var configurableSystem in configurableSystems)
        {
            var system = configurableSystem.Create.Invoke(_systemFactory);

            foreach (var subscription in configurableSystem.Subscriptions)
            {
                subscription.Invoke(_dispatcher, system);
            }
        }
    }
}
