using Centuriin.CardGame.Core.Engine.ECS.Systems;
using Centuriin.CardGame.Core.Engine.Events.Dispatching;
using Centuriin.CardGame.Core.Engine.Events.Domain;
using Centuriin.CardGame.Core.Engine.Loading.Configuration.Pipeline;

using FluentAssertions;

using Moq;

using Xunit;

namespace Centuriin.CardGame.Core.Engine.Tests.Loading.Configuration.Pipeline;

public sealed class ConfigurableGamePipelineTests
{
    [Fact(DisplayName = "Can be created as an empty pipeline.")]
    [Trait("Category", "Unit")]
    public void CanBeCreated()
    {
        // Arrange
        var pipeline = new ConfigurableGamePipeline();

        // Act
        var systems = pipeline.Freeze();

        // Assert
        systems.Should().BeEmpty();
    }

    [Fact(DisplayName = "Can append and insert systems before and after existing systems.")]
    [Trait("Category", "Unit")]
    public void CanConfigureSystemOrder()
    {
        // Arrange
        var pipeline = new ConfigurableGamePipeline();

        // Act
        pipeline.Add<SetupTurnFlowSystem>();
        pipeline.Add<TurnFlowSystem>();
        pipeline.AddAfter<SetupTurnFlowSystem, DealerSystem>();
        pipeline.AddBefore<TurnFlowSystem, CardMovementSystem>();
        var systems = pipeline.Freeze();

        // Assert
        systems.Select(x => x.SystemType).Should().Equal(
            typeof(SetupTurnFlowSystem),
            typeof(DealerSystem),
            typeof(CardMovementSystem),
            typeof(TurnFlowSystem));
    }

    [Fact(DisplayName = "Can replace a system at its current position with a new subscription configuration.")]
    [Trait("Category", "Unit")]
    public void CanReplaceSystem()
    {
        // Arrange
        var pipeline = new ConfigurableGamePipeline();
        pipeline.Add<SetupTurnFlowSystem>();
        pipeline.Add<DealerSystem>()
            .ForEvent<GameStartedEvent>();
        pipeline.Add<TurnFlowSystem>();

        // Act
        pipeline.Replace<DealerSystem, CardMovementSystem>()
            .ForEvent<CardDealtEvent>();
        var systems = pipeline.Freeze().ToArray();

        // Assert
        systems.Select(x => x.SystemType).Should().Equal(
            typeof(SetupTurnFlowSystem),
            typeof(CardMovementSystem),
            typeof(TurnFlowSystem));
        systems[1].ConsumingSubscriptions.Should().ContainSingle();
        systems[1].ProducingSubscriptions.Should().BeEmpty();
    }

    [Fact(DisplayName = "Can create the configured system through its typed factory.")]
    [Trait("Category", "Unit")]
    public void CanCreateConfiguredSystem()
    {
        // Arrange
        var pipeline = new ConfigurableGamePipeline();
        pipeline.Add<ISystem>();

        var expected = Mock.Of<ISystem>(MockBehavior.Strict);

        var factoryCalls = 0;
        var factory = new Mock<ISystemFactory>(MockBehavior.Strict);
        factory.Setup(x => x.Create<ISystem>())
            .Callback(() => factoryCalls++)
            .Returns(expected);

        // Act
        var system = pipeline.Freeze().Single().Create(factory.Object);

        // Assert
        system.Should().BeSameAs(expected);
        factoryCalls.Should().Be(1);
    }

    [Fact(DisplayName = "Can register both consuming and producing handlers for a selected event.")]
    [Trait("Category", "Unit")]
    public void CanRegisterSelectedEventHandlers()
    {
        // Arrange
        var pipeline = new ConfigurableGamePipeline();
        pipeline.Add<IDualSubscriberSystem>()
            .ForEvent<TurnEndedEvent>();

        var system = Mock.Of<IDualSubscriberSystem>(MockBehavior.Strict);

        var registrations = new List<string>();

        var consuming = new Mock<IConsumingSubscriberDispatcher>(MockBehavior.Strict);
        consuming.Setup(x => x.Register<TurnEndedEvent>(system))
            .Callback(() => registrations.Add("consume"));

        var producing = new Mock<IProducingSubscriberDispatcher>(MockBehavior.Strict);
        producing.Setup(x => x.Register<TurnEndedEvent>(system))
            .Callback(() => registrations.Add("produce"));

        var configured = pipeline.Freeze().Single();

        // Act
        foreach (var subscribe in configured.ConsumingSubscriptions)
        {
            subscribe(consuming.Object, system);
        }
        foreach (var subscribe in configured.ProducingSubscriptions)
        {
            subscribe(producing.Object, system);
        }

        // Assert
        registrations.Should().Equal("consume", "produce");
    }

    [Fact(DisplayName = "Can load the default game systems in the intended order.")]
    [Trait("Category", "Unit")]
    public void CanUseDefaultProfile()
    {
        // Arrange
        var pipeline = new ConfigurableGamePipeline();

        // Act
        pipeline.UseDefaultProfile();
        var systems = pipeline.Freeze();

        // Assert
        systems.Select(x => x.SystemType).Should().Equal(
            typeof(SetupTurnFlowSystem),
            typeof(DealerSystem),
            typeof(TurnFlowSystem),
            typeof(CardMovementSystem));
        systems.Sum(x => x.ConsumingSubscriptions.Count).Should().Be(3);
        systems.Sum(x => x.ProducingSubscriptions.Count).Should().Be(4);
    }

    [Fact(DisplayName = "Cannot configure the same system twice.")]
    [Trait("Category", "Unit")]
    public void CanNotAddDuplicateSystem()
    {
        // Arrange
        var pipeline = new ConfigurableGamePipeline();
        pipeline.Add<DealerSystem>();

        // Act
        var exception = Record.Exception(() =>
            pipeline.Add<DealerSystem>());

        // Assert
        exception.Should().BeOfType<InvalidOperationException>();
    }

    [Fact(DisplayName = "Cannot subscribe a system to the same event twice.")]
    [Trait("Category", "Unit")]
    public void CanNotAddDuplicateSubscription()
    {
        // Arrange
        var pipeline = new ConfigurableGamePipeline();

        var system = pipeline.Add<DealerSystem>()
            .ForEvent<GameStartedEvent>();

        // Act
        var exception = Record.Exception(() =>
            system
                .ForEvent<GameStartedEvent>());

        // Assert
        exception.Should().BeOfType<InvalidOperationException>();
    }

    [Fact(DisplayName = "Cannot subscribe a system to an event it does not handle.")]
    [Trait("Category", "Unit")]
    public void CanNotSubscribeToUnsupportedEvent()
    {
        // Arrange
        var pipeline = new ConfigurableGamePipeline();

        var system = pipeline.Add<DealerSystem>();

        // Act
        var exception = Record.Exception(() =>
            system
                .ForEvent<CardDealtEvent>());

        // Assert
        exception.Should().BeOfType<InvalidOperationException>();
    }

    [Fact(DisplayName = "Cannot insert after a system that does not exist.")]
    [Trait("Category", "Unit")]
    public void CanNotInsertAfterMissingSystem()
    {
        // Arrange
        var pipeline = new ConfigurableGamePipeline();

        // Act
        var exception = Record.Exception(() =>
            pipeline.AddAfter<DealerSystem, CardMovementSystem>());

        // Assert
        exception.Should().BeOfType<InvalidOperationException>();
    }

    [Fact(DisplayName = "Cannot insert before a system that does not exist.")]
    [Trait("Category", "Unit")]
    public void CanNotInsertBeforeMissingSystem()
    {
        // Arrange
        var pipeline = new ConfigurableGamePipeline();

        // Act
        var exception = Record.Exception(() =>
            pipeline.AddBefore<DealerSystem, CardMovementSystem>());

        // Assert
        exception.Should().BeOfType<InvalidOperationException>();
    }

    [Fact(DisplayName = "Cannot replace a system that does not exist.")]
    [Trait("Category", "Unit")]
    public void CanNotReplaceMissingSystem()
    {
        // Arrange
        var pipeline = new ConfigurableGamePipeline();

        // Act
        var exception = Record.Exception(() =>
            pipeline.Replace<DealerSystem, CardMovementSystem>());

        // Assert
        exception.Should().BeOfType<InvalidOperationException>();
    }

    [Fact(DisplayName = "Cannot replace a system with another system already in the pipeline.")]
    [Trait("Category", "Unit")]
    public void CanNotReplaceWithExistingSystem()
    {
        // Arrange
        var pipeline = new ConfigurableGamePipeline();
        pipeline.Add<DealerSystem>();
        pipeline.Add<CardMovementSystem>();

        // Act
        var exception = Record.Exception(() =>
            pipeline.Replace<DealerSystem, CardMovementSystem>());

        // Assert
        exception.Should().BeOfType<InvalidOperationException>();
    }

    [Fact(DisplayName = "Cannot change system order after the pipeline is frozen.")]
    [Trait("Category", "Unit")]
    public void CanNotChangeFrozenPipeline()
    {
        // Arrange
        var pipeline = new ConfigurableGamePipeline();
        pipeline.Add<DealerSystem>();
        _ = pipeline.Freeze();

        Action[] changes = [
            () => pipeline.Add<CardMovementSystem>(),
            () => pipeline.AddAfter<DealerSystem, CardMovementSystem>(),
            () => pipeline.AddBefore<DealerSystem, CardMovementSystem>(),
            () => pipeline.Replace<DealerSystem, CardMovementSystem>(),
            () => pipeline.UseDefaultProfile()
        ];

        // Act
        var exceptions = changes.Select(change => Record.Exception(change)).ToArray();

        // Assert
        exceptions.Should().AllBeOfType<InvalidOperationException>();
    }

    [Fact(DisplayName = "Cannot add subscriptions through a retained builder after freezing.")]
    [Trait("Category", "Unit")]
    public void CanNotChangeFrozenSubscriptions()
    {
        // Arrange
        var pipeline = new ConfigurableGamePipeline();

        var system = pipeline.Add<DealerSystem>();
        _ = pipeline.Freeze();

        // Act
        var exception = Record.Exception(() =>
            system
                .ForEvent<GameStartedEvent>());

        // Assert
        exception.Should().BeOfType<InvalidOperationException>();
    }

    public interface IDualSubscriberSystem :
        ISystem,
        IConsumingSubscriber<TurnEndedEvent>,
        IProducingSubscriber<TurnEndedEvent>;
}
