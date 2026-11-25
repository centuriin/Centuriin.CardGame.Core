using Centuriin.CardGame.Core.Engine.ECS.Systems;
using Centuriin.CardGame.Core.Engine.Events.Dispatching;
using Centuriin.CardGame.Core.Engine.Events.Domain;

namespace Centuriin.CardGame.Core.Engine.Loading.Configuration.Pipeline;

/// <summary>
/// Builds an ordered collection of game systems and their event subscriptions.
/// </summary>
internal sealed class ConfigurableGamePipeline : IConfigurableGamePipeline
{
    private bool _isFrozen;

    private readonly LinkedList<ConfigurableSystem> _steps = new();

    /// <summary>
    /// Prevents further configuration and returns the ordered system configurations.
    /// </summary>
    /// <returns>
    /// The completed configurations in their configured order.
    /// </returns>
    public IReadOnlyCollection<ConfigurableSystem> Freeze()
    {
        _isFrozen = true;

        return _steps;
    }

    /// <inheritdoc/>
    public IConfigurableGamePipeline UseDefaultProfile()
    {
        ThrowIfFrozen();

        DefaultProfile.Instance.Configure(this);

        return this;
    }

    /// <inheritdoc/>
    public IConfigurableSystem Add<TSystem>()
        where TSystem : ISystem
    {
        ThrowIfFrozen();

        ThrowIfExists<TSystem>();

        return _steps.AddLast(Create<TSystem>()).Value;
    }

    /// <inheritdoc/>
    public IConfigurableSystem AddAfter<TPrecedingSystem, TSystem>()
        where TPrecedingSystem : ISystem
        where TSystem : ISystem
    {
        ThrowIfFrozen();

        ThrowIfExists<TSystem>();

        var configurable = Create<TSystem>();

        _steps.AddAfter(GetNode<TPrecedingSystem>(), configurable);

        return configurable;
    }

    /// <inheritdoc/>
    public IConfigurableSystem AddBefore<TFollowingSystem, TSystem>()
        where TFollowingSystem : ISystem
        where TSystem : ISystem
    {
        ThrowIfFrozen();

        ThrowIfExists<TSystem>();

        var configurable = Create<TSystem>();

        _steps.AddBefore(GetNode<TFollowingSystem>(), configurable);

        return configurable;
    }

    /// <inheritdoc/>
    public IConfigurableSystem Replace<TReplaceableSystem, TSystem>()
        where TReplaceableSystem : ISystem
        where TSystem : ISystem
    {
        ThrowIfFrozen();

        ThrowIfExists<TSystem>();

        var node = GetNode<TReplaceableSystem>();
        if (typeof(TReplaceableSystem) != typeof(TSystem))
        {
            ThrowIfExists<TSystem>();
        }

        var configurable = Create<TSystem>();

        node.Value = configurable;

        return configurable;
    }

    private ConfigurableSystem Create<TSystem>()
        where TSystem : ISystem
        => new(this, typeof(TSystem), factory => factory.Create<TSystem>());

    private LinkedListNode<ConfigurableSystem>? FindNode<TSystem>()
        where TSystem : ISystem
    {
        for (var node = _steps.First; node is not null; node = node.Next)
        {
            if (node.Value.SystemType == typeof(TSystem))
            {
                return node;
            }
        }

        return null;
    }

    private LinkedListNode<ConfigurableSystem> GetNode<TSystem>()
        where TSystem : ISystem
        => FindNode<TSystem>()
            ?? throw new InvalidOperationException(
                $"System {typeof(TSystem).Name} does not exist.");

    private void ThrowIfFrozen()
    {
        if (_isFrozen)
        {
            throw new InvalidOperationException(
                "Game pipeline builder is already built.");
        }
    }

    private void ThrowIfExists<TSystem>()
        where TSystem : ISystem
    {
        if (FindNode<TSystem>() is not null)
        {
            throw new InvalidOperationException(
                $"System {typeof(TSystem).Name} already added.");
        }
    }

    /// <summary>
    /// Collects a system factory and its selected event subscriptions.
    /// </summary>
    /// <param name="configurator">
    /// The parent pipeline configuration.
    /// </param>
    /// <param name="systemType">
    /// The concrete type of the configured system.
    /// </param>
    /// <param name="create">
    /// The delegate used to create the configured system.
    /// </param>
    internal sealed class ConfigurableSystem(
        ConfigurableGamePipeline configurator,
        Type systemType,
        Func<ISystemFactory, ISystem> create) : IConfigurableSystem
    {
        private readonly HashSet<Type> _eventTypes = [];

        private readonly List<Action<IConsumingSubscriberDispatcher, ISystem>> _consumingSubscriptions = [];
        private readonly List<Action<IProducingSubscriberDispatcher, ISystem>> _producingSubscriptions = [];

        /// <inheritdoc/>
        public IConfigurableGamePipeline Pipeline => configurator;

        /// <summary>
        /// Gets the concrete type of the configured system.
        /// </summary>
        public Type SystemType => systemType;

        /// <summary>
        /// Gets the factory delegate used to create the configured system.
        /// </summary>
        public Func<ISystemFactory, ISystem> Create => create;

        /// <summary>
        /// Gets the registration actions for state-changing event subscribers.
        /// </summary>
        public IReadOnlyCollection<Action<IConsumingSubscriberDispatcher, ISystem>> ConsumingSubscriptions => _consumingSubscriptions;

        /// <summary>
        /// Gets the registration actions for event-producing subscribers.
        /// </summary>
        public IReadOnlyCollection<Action<IProducingSubscriberDispatcher, ISystem>> ProducingSubscriptions => _producingSubscriptions;

        /// <inheritdoc/>
        public IConfigurableSystem ForEvent<TEvent>()
            where TEvent : IGameEvent
        {
            configurator.ThrowIfFrozen();

            var eventType = typeof(TEvent);

            if (!_eventTypes.Add(eventType))
            {
                throw new InvalidOperationException(
                    $"Event {eventType.Name} for system" +
                    $" {systemType.Name} already added.");
            }

            if (!typeof(IConsumingSubscriber<TEvent>).IsAssignableFrom(systemType)
                && !typeof(IProducingSubscriber<TEvent>).IsAssignableFrom(systemType))
            {
                throw new InvalidOperationException(
                    $"System {systemType.Name} does not subscribe" +
                    $" to {eventType.Name}.");
            }

            if (typeof(IConsumingSubscriber<TEvent>).IsAssignableFrom(systemType))
            {
                _consumingSubscriptions.Add(
                    (dispatcher, system) =>
                        dispatcher.Register((IConsumingSubscriber<TEvent>)system));

            }
            if (typeof(IProducingSubscriber<TEvent>).IsAssignableFrom(systemType))
            {
                _producingSubscriptions.Add(
                    (dispatcher, system) =>
                        dispatcher.Register((IProducingSubscriber<TEvent>)system));
            }

            return this;
        }
    }
}
