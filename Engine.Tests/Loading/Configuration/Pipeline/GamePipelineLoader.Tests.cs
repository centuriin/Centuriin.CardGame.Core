using Centuriin.CardGame.Core.Engine.Context;
using Centuriin.CardGame.Core.Engine.ECS.Systems;
using Centuriin.CardGame.Core.Engine.Events.Dispatching;
using Centuriin.CardGame.Core.Engine.Events.Domain;
using Centuriin.CardGame.Core.Engine.Loading.Configuration;
using Centuriin.CardGame.Core.Engine.Loading.Configuration.Pipeline;

using FluentAssertions;

using Moq;

using Xunit;

namespace Centuriin.CardGame.Core.Engine.Tests.Loading.Configuration.Pipeline;

public sealed class GamePipelineLoaderTests
{
    [Fact(DisplayName = "Can be created.")]
    [Trait("Category", "Unit")]
    public void CanBeCreated()
    {
        // Act
        var exception = Record.Exception(() =>
            new GamePipelineLoader(
                Mock.Of<IGameContextAccessor<IGameProfileContext>>(MockBehavior.Strict),
                Mock.Of<IConsumingSubscriberDispatcher>(MockBehavior.Strict),
                Mock.Of<IProducingSubscriberDispatcher>(MockBehavior.Strict),
                Mock.Of<ISystemFactory>(MockBehavior.Strict)));

        // Assert
        exception.Should().BeNull();
    }

    [Fact(DisplayName = "Cannot be created without a profile context accessor.")]
    [Trait("Category", "Unit")]
    public void CanNotBeCreatedWithoutContextAccessor()
    {
        // Act
        var exception = Record.Exception(() =>
            new GamePipelineLoader(
                null!,
                Mock.Of<IConsumingSubscriberDispatcher>(MockBehavior.Strict),
                Mock.Of<IProducingSubscriberDispatcher>(MockBehavior.Strict),
                Mock.Of<ISystemFactory>(MockBehavior.Strict)));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = "Cannot be created without a consuming dispatcher.")]
    [Trait("Category", "Unit")]
    public void CanNotBeCreatedWithoutConsumingDispatcher()
    {
        // Act
        var exception = Record.Exception(() =>
            new GamePipelineLoader(
                Mock.Of<IGameContextAccessor<IGameProfileContext>>(MockBehavior.Strict),
                null!,
                Mock.Of<IProducingSubscriberDispatcher>(MockBehavior.Strict),
                Mock.Of<ISystemFactory>(MockBehavior.Strict)));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = "Cannot be created without a producing dispatcher.")]
    [Trait("Category", "Unit")]
    public void CanNotBeCreatedWithoutProducingDispatcher()
    {
        // Act
        var exception = Record.Exception(() =>
            new GamePipelineLoader(
                Mock.Of<IGameContextAccessor<IGameProfileContext>>(MockBehavior.Strict),
                Mock.Of<IConsumingSubscriberDispatcher>(MockBehavior.Strict),
                null!,
                Mock.Of<ISystemFactory>(MockBehavior.Strict)));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = "Cannot be created without a system factory.")]
    [Trait("Category", "Unit")]
    public void CanNotBeCreatedWithoutSystemFactory()
    {
        // Act
        var exception = Record.Exception(() =>
            new GamePipelineLoader(
                Mock.Of<IGameContextAccessor<IGameProfileContext>>(MockBehavior.Strict),
                Mock.Of<IConsumingSubscriberDispatcher>(MockBehavior.Strict),
                Mock.Of<IProducingSubscriberDispatcher>(MockBehavior.Strict),
                null!));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = "Can create systems in pipeline order and register only their configured handlers.")]
    [Trait("Category", "Unit")]
    public async Task CanLoadConfiguredSystemsAsync()
    {
        // Arrange
        var gameId = new GameId(Guid.NewGuid());
        var token = TestContext.Current.CancellationToken;

        var turn = Mock.Of<ITurnSystem>(MockBehavior.Strict);

        var movement = Mock.Of<ICardSystem>(MockBehavior.Strict);

        var calls = new List<string>();

        var profile = new Mock<IGameProfile>(MockBehavior.Strict);
        profile.Setup(x => x.Configure(It.IsAny<IConfigurableGamePipeline>()))
            .Callback<IConfigurableGamePipeline>(pipeline =>
            {
                pipeline.Add<ITurnSystem>()
                    .ForEvent<TurnEndedEvent>();

                pipeline.Add<ICardSystem>()
                    .ForEvent<CardDealtEvent>();
            });

        var context = new Mock<IGameProfileContext>(MockBehavior.Strict);
        context.SetupGet(x => x.Profile)
            .Returns(profile.Object);

        var accessor = new Mock<IGameContextAccessor<IGameProfileContext>>(MockBehavior.Strict);
        accessor.Setup(x => x.GetContextAsync(gameId, token))
            .ReturnsAsync(context.Object);

        var factory = new Mock<ISystemFactory>(MockBehavior.Strict);
        factory.Setup(x => x.Create<ITurnSystem>())
            .Callback(() => calls.Add("create-turn"))
            .Returns(turn);
        factory.Setup(x => x.Create<ICardSystem>())
            .Callback(() => calls.Add("create-movement"))
            .Returns(movement);

        var consuming = new Mock<IConsumingSubscriberDispatcher>(MockBehavior.Strict);
        consuming.Setup(x => x.Register<TurnEndedEvent>(turn))
            .Callback(() => calls.Add("consume-turn"));
        consuming.Setup(x => x.Register<CardDealtEvent>(movement))
            .Callback(() => calls.Add("consume-movement"));

        var producing = new Mock<IProducingSubscriberDispatcher>(MockBehavior.Strict);
        producing.Setup(x => x.Register<TurnEndedEvent>(turn))
            .Callback(() => calls.Add("produce-turn"));

        var loader = new GamePipelineLoader(
            accessor.Object,
            consuming.Object,
            producing.Object,
            factory.Object);

        // Act
        await loader.LoadAsync(gameId, token);

        // Assert
        calls.Should().Equal(
            "create-turn",
            "consume-turn",
            "produce-turn",
            "create-movement",
            "consume-movement");
    }

    [Fact(DisplayName = "Cannot load a pipeline after cancellation or access any dependencies.")]
    [Trait("Category", "Unit")]
    public async Task CanNotLoadAfterCancellationAsync()
    {
        // Arrange
        using var cancellation = CancellationTokenSource.CreateLinkedTokenSource(TestContext.Current.CancellationToken);
        cancellation.Cancel();

        var loader = new GamePipelineLoader(
            Mock.Of<IGameContextAccessor<IGameProfileContext>>(MockBehavior.Strict),
            Mock.Of<IConsumingSubscriberDispatcher>(MockBehavior.Strict),
            Mock.Of<IProducingSubscriberDispatcher>(MockBehavior.Strict),
            Mock.Of<ISystemFactory>(MockBehavior.Strict));

        // Act
        var exception = await Record.ExceptionAsync(() =>
            loader.LoadAsync(new(Guid.NewGuid()), cancellation.Token));

        // Assert
        exception.Should().BeAssignableTo<OperationCanceledException>();
    }

    [Fact(DisplayName = "Can propagate profile loading failures before creating systems.")]
    [Trait("Category", "Unit")]
    public async Task CanPropagateProfileFailureAsync()
    {
        // Arrange
        var gameId = new GameId(Guid.NewGuid());
        var token = TestContext.Current.CancellationToken;

        var failure = new InvalidOperationException("Profile unavailable.");

        var accessor = new Mock<IGameContextAccessor<IGameProfileContext>>(MockBehavior.Strict);
        accessor.Setup(x => x.GetContextAsync(gameId, token))
            .ThrowsAsync(failure);

        var loader = new GamePipelineLoader(
            accessor.Object,
            Mock.Of<IConsumingSubscriberDispatcher>(MockBehavior.Strict),
            Mock.Of<IProducingSubscriberDispatcher>(MockBehavior.Strict),
            Mock.Of<ISystemFactory>(MockBehavior.Strict));

        // Act
        var exception = await Record.ExceptionAsync(() =>
            loader.LoadAsync(gameId, token));

        // Assert
        exception.Should().BeSameAs(failure);
    }

    public interface ITurnSystem :
        ISystem,
        IConsumingSubscriber<TurnEndedEvent>,
        IProducingSubscriber<TurnEndedEvent>;

    public interface ICardSystem :
        ISystem,
        IConsumingSubscriber<CardDealtEvent>;
}
