using Centuriin.CardGame.Core.Common.Events;
using Centuriin.CardGame.Core.Common.Events.Dispatching;
using Centuriin.CardGame.Core.Common.Factories;
using Centuriin.CardGame.Core.Common.Systems;

namespace Centuriin.CardGame.Core.Common.GameProfiles;

public sealed class GamePipelineBuilder : IGamePipelineBuilder
{
    private readonly LinkedList<RegistrationStep> _steps = new();
    private readonly ISystemFactory _factory;
    private readonly IEventDispatcher _dispatcher;
    private bool _isBuilded;

    public GamePipelineBuilder(
        ISystemFactory factory,
        IEventDispatcher dispatcher)
    {
        ArgumentNullException.ThrowIfNull(factory);
        _factory = factory;

        ArgumentNullException.ThrowIfNull(dispatcher);
        _dispatcher = dispatcher;
    }

    public IConfigurableGamePipeline UseDefaultProfile()
    {
        DefaultProfile.Instance.Configure(this);
        return this;
    }

    public IConfigurableGamePipeline Add<TSystem, TEvent>()
        where TSystem : SystemBase, ISubscriber<TEvent>
        where TEvent : IGameEvent
    {
        ThrowIfBuilded();

        _steps.AddLast(CreateStep<TSystem, TEvent>());
        return this;
    }

    public IConfigurableGamePipeline AddAfter<TBellowSystem, TSystem, TEvent>()
        where TBellowSystem : SystemBase
        where TSystem : SystemBase, ISubscriber<TEvent>
        where TEvent : IGameEvent
    {
        ThrowIfBuilded();

        var node = FindNode<TBellowSystem>();
        _steps.AddAfter(node, CreateStep<TSystem, TEvent>());
        return this;
    }

    public IConfigurableGamePipeline AddBefore<TFollowSystem, TSystem, TEvent>()
        where TFollowSystem : SystemBase
        where TSystem : SystemBase, ISubscriber<TEvent>
        where TEvent : IGameEvent
    {
        ThrowIfBuilded();

        var node = FindNode<TFollowSystem>();
        _steps.AddBefore(node, CreateStep<TSystem, TEvent>());
        return this;
    }

    public IConfigurableGamePipeline Replace<TReplaceableSystem, TSystem, TEvent>()
        where TReplaceableSystem : SystemBase
        where TSystem : SystemBase, ISubscriber<TEvent>
        where TEvent : IGameEvent
    {
        ThrowIfBuilded();

        var node = FindNode<TReplaceableSystem>();
        node.Value = CreateStep<TSystem, TEvent>();
        return this;
    }

    public void Build()
    {
        ThrowIfBuilded();

        var instancesCache = new Dictionary<Type, SystemBase>(_steps.Count);

        foreach (var step in _steps)
        {
            if (!instancesCache.TryGetValue(step.SystemType, out var system))
            {
                system = _factory.Create(step.SystemType);
                instancesCache[step.SystemType] = system;
            }

            step.RegisterAction.Invoke(_dispatcher, system);
        }

        _isBuilded = true;
    }

    private void ThrowIfBuilded()
    {
        if (_isBuilded)
        {
            throw new InvalidOperationException();
        }
    }

    private static RegistrationStep CreateStep<TSystem, TEvent>()
        where TSystem : SystemBase, ISubscriber<TEvent>
        where TEvent : IGameEvent
    {
        return new RegistrationStep(
            typeof(TSystem),
            typeof(TEvent),
            (dispatcher, system) => dispatcher.Register((TSystem)system)
        );
    }

    private LinkedListNode<RegistrationStep> FindNode<TSystem>()
    {
        var current = _steps.Last;
        while (current != null)
        {
            if (current.Value.SystemType == typeof(TSystem))
                return current;
            current = current.Previous;
        }
        throw new InvalidOperationException($"System {typeof(TSystem).Name} not found in pipeline.");
    }

    private sealed record RegistrationStep(
        Type SystemType,
        Type EventType,
        Action<IEventDispatcher, SystemBase> RegisterAction);
}