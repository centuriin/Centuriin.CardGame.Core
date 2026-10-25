using Centuriin.CardGame.Core.Common.ECS.Systems;
using Centuriin.CardGame.Core.Common.Events.Dispatching;
using Centuriin.CardGame.Core.Common.Events.Domain;
using Centuriin.CardGame.Core.Common.GameProfiles;

namespace Centuriin.CardGame.Core.Common.Configuration.Pipeline;

public sealed class GamePipelineConfigurator : ConfiguratorBase, IGamePipelineConfigurator
{
    private readonly LinkedList<ConfigurableSystem> _steps = new();

    private readonly ISystemFactory _factory;
    private readonly IDispatcher<IGameEvent> _dispatcher;

    public GamePipelineConfigurator(
        ISystemFactory factory,
        IDispatcher<IGameEvent> dispatcher)
    {
        ArgumentNullException.ThrowIfNull(factory);
        _factory = factory;

        ArgumentNullException.ThrowIfNull(dispatcher);
        _dispatcher = dispatcher;
    }
    public IConfigurableGamePipeline UseDefaultProfile()
    {
        ThrowIfConfigured();

        DefaultProfile.Instance.Configure(this);
        return this;
    }

    /// <inheritdoc/>
    public IConfigurableSystem Add<TSystem>()
        where TSystem : SystemBase
    {
        ThrowIfConfigured();

        if (FindNode<TSystem>() is { })
        {
            throw new InvalidOperationException(
                $"System {typeof(TSystem).Name} already added.");
        }

        var configurable = CreateConfigurableSystem<TSystem>();

        return _steps.AddLast(configurable).Value;
    }

    /// <inheritdoc/>
    public IConfigurableSystem AddAfter<TBellowSystem, TSystem>()
        where TBellowSystem : SystemBase
        where TSystem : SystemBase
    {
        ThrowIfConfigured();

        var configurable = CreateConfigurableSystem<TSystem>();

        _steps.AddAfter(
            GetNode<TBellowSystem>(),
            configurable);

        return configurable;
    }

    /// <inheritdoc/>
    public IConfigurableSystem AddBefore<TFollowSystem, TSystem>()
        where TFollowSystem : SystemBase
        where TSystem : SystemBase
    {
        ThrowIfConfigured();

        var configurable = CreateConfigurableSystem<TSystem>();

        _steps.AddBefore(
            GetNode<TFollowSystem>(),
            configurable);

        return configurable;
    }

    /// <inheritdoc/>
    public IConfigurableSystem Replace<TReplaceableSystem, TSystem>()
        where TReplaceableSystem : SystemBase
        where TSystem : SystemBase
    {
        ThrowIfConfigured();

        var node = GetNode<TReplaceableSystem>();

        var configurable = CreateConfigurableSystem<TSystem>();

        node.Value = configurable;

        return configurable;
    }

    /// <inheritdoc/>
    protected override void ConfigureCore()
    {
        foreach (var step in _steps)
        {
            foreach (var action in step.Actions.Values)
            {
                action.Invoke();
            }
        }
    }

    private LinkedListNode<ConfigurableSystem>? FindNode<TSystem>()
    {
        var current = _steps.Last;
        while (current != null)
        {
            if (current.Value.System.GetType() == typeof(TSystem))
                return current;
            current = current.Previous;
        }

        return null;
    }

    private LinkedListNode<ConfigurableSystem> GetNode<TSystem>()
        => FindNode<TSystem>()
            ?? throw new InvalidOperationException(
                $"System {typeof(TSystem).Name} does not exist.");

    private ConfigurableSystem CreateConfigurableSystem<TSystem>()
        where TSystem : SystemBase
    {
        var system = _factory.Create<TSystem>();

        return new()
        {
            System = system,
            Configurator = this
        };
    }

    private sealed class ConfigurableSystem : IConfigurableSystem
    {
        public Dictionary<Type, Action> Actions { get; } = [];

        public required SystemBase System { get; init; }

        public required GamePipelineConfigurator Configurator { get; init; }

        public IConfigurableGamePipeline GamePipeline => Configurator;

        public IConfigurableSystem ForEvent<TEvent>()
            where TEvent : IGameEvent
        {
            var eventType = typeof(TEvent);

            if (Actions.TryGetValue(eventType, out var _))
            {
                throw new InvalidOperationException(
                    $"Event {eventType.Name} for system {System.GetType().Name}" +
                    $" already added.");
            }

            var subscriber = (ISubscriber<TEvent>)System;

            Actions[eventType] = () => Configurator._dispatcher.Register(subscriber);

            return this;
        }
    }
}