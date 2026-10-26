using Centuriin.CardGame.Core.Common.ECS.Systems;
using Centuriin.CardGame.Core.Common.Events.Dispatching;
using Centuriin.CardGame.Core.Common.Events.Domain;
using Centuriin.CardGame.Core.Common.GameProfiles;

namespace Centuriin.CardGame.Core.Common.Configuration.Pipeline;

internal sealed class ConfigurableGamePipeline : IGamePipeline
{
    private bool _isFrozen;

    private readonly LinkedList<ConfigurableSystem> _steps = new();

    public IConfigurableGamePipeline UseDefaultProfile()
    {
        ThrowIfFrozen();

        DefaultProfile.Instance.Configure(this);

        return this;
    }

    public IConfigurableSystem Add<TSystem>() 
        where TSystem : SystemBase
    {
        ThrowIfFrozen();

        ThrowIfExists<TSystem>();

        return _steps.AddLast(Create<TSystem>()).Value;
    }

    public IConfigurableSystem AddAfter<TBellowSystem, TSystem>()
        where TBellowSystem : SystemBase 
        where TSystem : SystemBase
    {
        ThrowIfFrozen();

        ThrowIfExists<TSystem>();

        var configurable = Create<TSystem>();

        _steps.AddAfter(GetNode<TBellowSystem>(), configurable);

        return configurable;
    }

    public IConfigurableSystem AddBefore<TFollowSystem, TSystem>()
        where TFollowSystem : SystemBase where TSystem : SystemBase
    {
        ThrowIfFrozen();

        ThrowIfExists<TSystem>();

        var configurable = Create<TSystem>();

        _steps.AddAfter(GetNode<TFollowSystem>(), configurable);

        return configurable;
    }

    public IConfigurableSystem Replace<TReplaceableSystem, TSystem>()
        where TReplaceableSystem : SystemBase 
        where TSystem : SystemBase
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

    
    
    public void Freeze() => _isFrozen = true;

    public void Apply(IDispatcher<IGameEvent> dispatcher, ISystemFactory factory)
    {
        if (!_isFrozen)
        {
            throw new InvalidOperationException("Configuration must be frozen.");
        }

        foreach (var step in _steps)
        {
            var system = step.Create.Invoke(factory);

            foreach (var subscribe in step.Subscriptions.Values)
            {
                subscribe.Invoke(dispatcher, system);
            }   
        }
    }

    private ConfigurableSystem Create<TSystem>() 
        where TSystem : SystemBase 
        => new(this, typeof(TSystem), factory => factory.Create<TSystem>());

    private void ThrowIfFrozen()
    {
        if (_isFrozen)
        {
            throw new InvalidOperationException(
                "Game pipeline builder is already built.");
        }
    }

    
    
    private void ThrowIfExists<TSystem>() where TSystem : SystemBase
    {
        if (FindNode<TSystem>() is not null)
        {
            throw new InvalidOperationException(
                $"System {typeof(TSystem).Name} already added.");
        }
    }

    private LinkedListNode<ConfigurableSystem>? FindNode<TSystem>() 
        where TSystem : SystemBase
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
        where TSystem : SystemBase 
        => FindNode<TSystem>() 
            ?? throw new InvalidOperationException(
                $"System {typeof(TSystem).Name} does not exist.");

    private sealed class ConfigurableSystem(
        ConfigurableGamePipeline configurator,
        Type systemType,
        Func<ISystemFactory, SystemBase> create) : IConfigurableSystem
    {
        public IConfigurableGamePipeline GamePipeline => configurator;

        public Type SystemType => systemType;
        
        public Func<ISystemFactory, SystemBase> Create => create;
        
        public Dictionary<Type, Action<IDispatcher<IGameEvent>, SystemBase>> Subscriptions { get; } = [];        

        public IConfigurableSystem ForEvent<TEvent>() 
            where TEvent : IGameEvent
        {
            configurator.ThrowIfFrozen();

            var eventType = typeof(TEvent);

            if (!typeof(ISubscriber<TEvent>).IsAssignableFrom(systemType))
            {

                throw new InvalidOperationException(
                    $"System {systemType.Name} does not subscribe" +
                    $" to {eventType.Name}.");
            }

            if (Subscriptions.ContainsKey(eventType))
            {
                throw new InvalidOperationException(
                    $"Event {eventType.Name} for system" +
                    $" {systemType.Name} already added.");
            }

            Subscriptions[eventType] =
                (dispatcher, system) => 
                    dispatcher.Register((ISubscriber<TEvent>)system);

            return this;
        }
    }
}
