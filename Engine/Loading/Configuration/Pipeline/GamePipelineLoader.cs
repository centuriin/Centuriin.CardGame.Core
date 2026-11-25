using Centuriin.CardGame.Core.Engine.Context;
using Centuriin.CardGame.Core.Engine.ECS.Systems;
using Centuriin.CardGame.Core.Engine.Events.Dispatching;

namespace Centuriin.CardGame.Core.Engine.Loading.Configuration.Pipeline;

/// <summary>
/// Creates configured game systems and registers their event subscriptions.
/// </summary>
public sealed class GamePipelineLoader : IGameLoader
{
    private readonly IGameContextAccessor<IGameProfileContext> _profileContextAccessor;
    private readonly IConsumingSubscriberDispatcher _consumingDispatcher;
    private readonly IProducingSubscriberDispatcher _producingDispatcher;
    private readonly ISystemFactory _systemFactory;
    private readonly ConfigurableGamePipeline _configurableGamePipeline = new();

    /// <summary>
    /// Initializes a new <see cref="GamePipelineLoader"/> instance.
    /// </summary>
    /// <param name="profileContextAccessor">
    /// The accessor used to load the game profile.
    /// </param>
    /// <param name="consumingDispatcher">
    /// The dispatcher used to invoke state-changing subscribers.
    /// </param>
    /// <param name="producingDispatcher">
    /// The dispatcher used to invoke event-producing subscribers.
    /// </param>
    /// <param name="systemFactory">
    /// The factory used to create configured game systems.
    /// </param>
    public GamePipelineLoader(
        IGameContextAccessor<IGameProfileContext> profileContextAccessor,
        IConsumingSubscriberDispatcher consumingDispatcher,
        IProducingSubscriberDispatcher producingDispatcher,
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

    /// <inheritdoc/>
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
