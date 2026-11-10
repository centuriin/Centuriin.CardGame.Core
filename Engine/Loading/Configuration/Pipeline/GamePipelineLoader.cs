using Centuriin.CardGame.Core.Engine.Context;
using Centuriin.CardGame.Core.Engine.ECS.Systems;
using Centuriin.CardGame.Core.Engine.Events.Dispatching;

namespace Centuriin.CardGame.Core.Engine.Loading.Configuration.Pipeline;

public sealed class GamePipelineLoader : IGameLoader
{
    private readonly IGameContextAccessor<IGameProfileContext> _profileContextAccessor;
    private readonly IConsumingSubscriberDispatcher _consumingDispatcher;
    private readonly IProducingSubcriberDispatcher _producingDispatcher;
    private readonly ISystemFactory _systemFactory;
    private readonly ConfigurableGamePipeline _configurableGamePipeline = new();

    public GamePipelineLoader(
        IGameContextAccessor<IGameProfileContext> profileContextAccessor,
        IConsumingSubscriberDispatcher consumingDispatcher,
        IProducingSubcriberDispatcher producingDispatcher,
        ISystemFactory systemFactory)
    {
        ArgumentNullException.ThrowIfNull(profileContextAccessor);
        _profileContextAccessor = profileContextAccessor;

        ArgumentNullException.ThrowIfNull(consumingDispatcher);
        _consumingDispatcher = consumingDispatcher;

        ArgumentNullException.ThrowIfNull(producingDispatcher);
        _producingDispatcher = producingDispatcher;

        ArgumentNullException.ThrowIfNull(systemFactory);
        _systemFactory = systemFactory;
    }

    public async Task LoadAsync(GameId gameId, CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        var profile = (await _profileContextAccessor.GetContextAsync(gameId, token))
            .Profile;

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
