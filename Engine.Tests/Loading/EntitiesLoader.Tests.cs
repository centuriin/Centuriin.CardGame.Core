using Centuriin.CardGame.Core.Engine.Context;
using Centuriin.CardGame.Core.Engine.ECS.Entities;
using Centuriin.CardGame.Core.Engine.ECS.Entities.Factories;
using Centuriin.CardGame.Core.Engine.Loading;
using Centuriin.CardGame.Core.Engine.Root.State;

using FluentAssertions;

using Moq;

using Xunit;

namespace Centuriin.CardGame.Core.Engine.Tests.Loading;

public sealed class EntitiesLoaderTests
{
    [Fact(DisplayName = "Can be created.")]
    [Trait("Category", "Unit")]
    public void CanBeCreated()
    {
        // Act
        var exception = Record.Exception(() =>
            new EntitiesLoader(
                Mock.Of<IGameState>(MockBehavior.Strict),
                Mock.Of<IEntityIdProvider>(MockBehavior.Strict),
                Mock.Of<IGameContextAccessor<IReadOnlyInitialStateContext>>(MockBehavior.Strict),
                []));

        // Assert
        exception.Should().BeNull();
    }

    [Fact(DisplayName = "Cannot be created without game state.")]
    [Trait("Category", "Unit")]
    public void CanNotBeCreatedWithoutGameState()
    {
        // Act
        var exception = Record.Exception(() =>
            new EntitiesLoader(
                null!,
                Mock.Of<IEntityIdProvider>(MockBehavior.Strict),
                Mock.Of<IGameContextAccessor<IReadOnlyInitialStateContext>>(MockBehavior.Strict),
                []));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = "Cannot be created without an entity identifier provider.")]
    [Trait("Category", "Unit")]
    public void CanNotBeCreatedWithoutEntityIdProvider()
    {
        // Act
        var exception = Record.Exception(() =>
            new EntitiesLoader(
                Mock.Of<IGameState>(MockBehavior.Strict),
                null!,
                Mock.Of<IGameContextAccessor<IReadOnlyInitialStateContext>>(MockBehavior.Strict),
                []));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = "Cannot be created without an initial state context accessor.")]
    [Trait("Category", "Unit")]
    public void CanNotBeCreatedWithoutContextAccessor()
    {
        // Act
        var exception = Record.Exception(() =>
            new EntitiesLoader(
                Mock.Of<IGameState>(MockBehavior.Strict),
                Mock.Of<IEntityIdProvider>(MockBehavior.Strict),
                null!,
                []));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = "Cannot be created without an entity loader collection.")]
    [Trait("Category", "Unit")]
    public void CanNotBeCreatedWithoutEntityLoaders()
    {
        // Act
        var exception = Record.Exception(() =>
            new EntitiesLoader(
                Mock.Of<IGameState>(MockBehavior.Strict),
                Mock.Of<IEntityIdProvider>(MockBehavior.Strict),
                Mock.Of<IGameContextAccessor<IReadOnlyInitialStateContext>>(MockBehavior.Strict),
                null!));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = "Can load all supported entities before restoring the identifier sequence.")]
    [Trait("Category", "Unit")]
    public async Task CanLoadEntitiesAndRestoreIdentifierAsync()
    {
        // Arrange
        var token = TestContext.Current.CancellationToken;
        var gameId = new GameId(Guid.NewGuid());

        var state = Mock.Of<IGameState>(MockBehavior.Strict);

        var entities = Mock.Of<IReadOnlyEntitiesCollection>(MockBehavior.Strict);

        var context = new Mock<IReadOnlyInitialStateContext>(MockBehavior.Strict);
        context.SetupGet(x => x.Entities)
            .Returns(entities);
        context.SetupGet(x => x.LastEntityId)
            .Returns(new EntityId(42));

        var accessor = new Mock<IGameContextAccessor<IReadOnlyInitialStateContext>>(MockBehavior.Strict);
        accessor.Setup(x => x.GetContextAsync(gameId, token))
            .ReturnsAsync(context.Object);

        var calls = new List<string>();
        var first = new Mock<IEntityLoader>(MockBehavior.Strict);
        first.Setup(x => x.Load(entities, state))
            .Callback(() => calls.Add("first"));

        var second = new Mock<IEntityLoader>(MockBehavior.Strict);
        second.Setup(x => x.Load(entities, state))
            .Callback(() => calls.Add("second"));

        var provider = new Mock<IEntityIdProvider>(MockBehavior.Strict);
        provider.Setup(x => x.SetInitialId(new EntityId(42)))
            .Callback(() => calls.Add("identifier"));

        var loader = new EntitiesLoader(
            state,
            provider.Object,
            accessor.Object,
            [first.Object, second.Object]);

        // Act
        await loader.LoadAsync(gameId, token);

        // Assert
        calls.Should().Equal("first", "second", "identifier");
    }

    [Fact(DisplayName = "Cannot load entities after cancellation or access any dependencies.")]
    [Trait("Category", "Unit")]
    public async Task CanNotLoadAfterCancellationAsync()
    {
        // Arrange
        using var cancellation = CancellationTokenSource.CreateLinkedTokenSource(TestContext.Current.CancellationToken);
        cancellation.Cancel();

        var loader = new EntitiesLoader(
            Mock.Of<IGameState>(MockBehavior.Strict),
            Mock.Of<IEntityIdProvider>(MockBehavior.Strict),
            Mock.Of<IGameContextAccessor<IReadOnlyInitialStateContext>>(MockBehavior.Strict),
            [Mock.Of<IEntityLoader>(MockBehavior.Strict)]);

        // Act
        var exception = await Record.ExceptionAsync(() =>
            loader.LoadAsync(new(Guid.NewGuid()), cancellation.Token));

        // Assert
        exception.Should().BeAssignableTo<OperationCanceledException>();
    }

    [Fact(DisplayName = "Can propagate a context loading failure without loading entities or resetting identifiers.")]
    [Trait("Category", "Unit")]
    public async Task CanPropagateContextFailureAsync()
    {
        // Arrange
        var gameId = new GameId(Guid.NewGuid());
        var token = TestContext.Current.CancellationToken;

        var failure = new InvalidOperationException("Context unavailable.");

        var accessor = new Mock<IGameContextAccessor<IReadOnlyInitialStateContext>>(MockBehavior.Strict);
        accessor.Setup(x => x.GetContextAsync(gameId, token))
            .ThrowsAsync(failure);

        var loader = new EntitiesLoader(
            Mock.Of<IGameState>(MockBehavior.Strict),
            Mock.Of<IEntityIdProvider>(MockBehavior.Strict),
            accessor.Object,
            [Mock.Of<IEntityLoader>(MockBehavior.Strict)]);

        // Act
        var exception = await Record.ExceptionAsync(() =>
            loader.LoadAsync(gameId, token));

        // Assert
        exception.Should().BeSameAs(failure);
    }

    [Fact(DisplayName = "Can stop loading on the first entity loader failure without resetting identifiers.")]
    [Trait("Category", "Unit")]
    public async Task CanPropagateEntityLoaderFailureAsync()
    {
        // Arrange
        var gameId = new GameId(Guid.NewGuid());
        var token = TestContext.Current.CancellationToken;

        var state = Mock.Of<IGameState>(MockBehavior.Strict);

        var entities = Mock.Of<IReadOnlyEntitiesCollection>(MockBehavior.Strict);

        var failure = new InvalidOperationException("Entity loading failed.");

        var context = new Mock<IReadOnlyInitialStateContext>(MockBehavior.Strict);
        context.SetupGet(x => x.Entities)
            .Returns(entities);

        var accessor = new Mock<IGameContextAccessor<IReadOnlyInitialStateContext>>(MockBehavior.Strict);
        accessor.Setup(x => x.GetContextAsync(gameId, token))
            .ReturnsAsync(context.Object);

        var first = new Mock<IEntityLoader>(MockBehavior.Strict);
        first.Setup(x => x.Load(entities, state))
            .Throws(failure);

        var loader = new EntitiesLoader(
            state,
            Mock.Of<IEntityIdProvider>(MockBehavior.Strict),
            accessor.Object,
            [first.Object, Mock.Of<IEntityLoader>(MockBehavior.Strict)]);

        // Act
        var exception = await Record.ExceptionAsync(() =>
            loader.LoadAsync(gameId, token));

        // Assert
        exception.Should().BeSameAs(failure);
    }
}
