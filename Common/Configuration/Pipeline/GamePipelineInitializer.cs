using Centuriin.CardGame.Core.Common.ECS.Systems;
using Centuriin.CardGame.Core.Common.Events.Dispatching;

namespace Centuriin.CardGame.Core.Common.Configuration.Pipeline;

public sealed class GamePipelineInitializer : IGameConfigurationInitializer
{
    private readonly IConsumingSubscriberDispatcher _consumingDispatcher;
    private readonly IProducingSubcriberDispatcher _producingDispatcher;
    private readonly ISystemFactory _systemFactory;
    private readonly ConfigurableGamePipeline _configurableGamePipeline = new();

    public GamePipelineInitializer(
        IConsumingSubscriberDispatcher consumingDispatcher,
        IProducingSubcriberDispatcher producingDispatcher,
        ISystemFactory systemFactory)
    {
        ArgumentNullException.ThrowIfNull(consumingDispatcher);
        _consumingDispatcher = consumingDispatcher;

        ArgumentNullException.ThrowIfNull(producingDispatcher);
        _producingDispatcher = producingDispatcher;

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

            foreach (var subscription in configurableSystem.ConsumingSubscriptions)
            {
                subscription.Invoke(_consumingDispatcher, system);
            }

            foreach (var subscription in configurableSystem.ProducingSubscriptions)
            {
                subscription.Invoke(_producingDispatcher, system);
            }
        }
    }
}
