using Centuriin.CardGame.Core.Common.Events;
using Centuriin.CardGame.Core.Common.Factories;
using Centuriin.CardGame.Core.Common.Loaders;
using Centuriin.Centuriin.Core.Common;

using FluentAssertions;

using Moq;

using Xunit;

namespace Centuriin.CardGame.Core.Common;

public sealed class GameStartupServiceTests
{
    [Fact]
    public void ConstructorShouldThrowWhenLoadersIsNull()
    {
        // Act
        var exception = Record.Exception(() =>
            new GameStartupService(null!, Mock.Of<IGameFactory>()));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact]
    public void ConstructorShouldThrowWhenFactoryIsNull()
    {
        // Act
        var exception = Record.Exception(() =>
            new GameStartupService([], null!));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact]
    public async Task StartupGameAsyncShouldCreateGameLoadDataAndApplyStartedEventAsync()
    {
        // Arrange
        var gameId = new GameId(Guid.NewGuid());
        var setup = new GameSetup(new(1), []);

        var gameStateMock = new Mock<IGameState>(MockBehavior.Strict);
        gameStateMock.SetupGet(x => x.GameId).Returns(gameId);

        var applyCalls = 0;
        var gameMock = new Mock<IGame>(MockBehavior.Strict);
        gameMock.SetupGet(x => x.State).Returns(gameStateMock.Object);
        gameMock
            .Setup(x => x.ApplyAsync(
                It.Is<GameStartedEvent>(e => e.GameId == gameId),
                TestContext.Current.CancellationToken))
            .Callback(() => applyCalls++)
            .Returns(Task.CompletedTask);

        var factoryCalls = 0;
        var factoryMock = new Mock<IGameFactory>(MockBehavior.Strict);
        factoryMock
            .Setup(x => x.Create(setup))
            .Callback(() => factoryCalls++)
            .Returns(gameMock.Object);

        var loaderCalls = 0;
        var loaderMock = new Mock<IGameLoader>(MockBehavior.Strict);
        loaderMock
            .Setup(x => x.LoadAsync(setup, gameStateMock.Object, TestContext.Current.CancellationToken))
            .Callback(() => loaderCalls++)
            .Returns(Task.CompletedTask);

        var service = new GameStartupService([loaderMock.Object], factoryMock.Object);

        // Act
        var result = await service.StartupGameAsync(setup, TestContext.Current.CancellationToken);

        // Assert
        result.Should().BeSameAs(gameMock.Object);
        factoryCalls.Should().Be(1);
        loaderCalls.Should().Be(1);
        applyCalls.Should().Be(1);
    }

    [Fact]
    public async Task StartupGameAsyncShouldThrowWhenSetupIsNullAsync()
    {
        // Arrange
        var service = new GameStartupService([], Mock.Of<IGameFactory>());

        // Act
        var exception = await Record.ExceptionAsync(() =>
            service.StartupGameAsync(null!, TestContext.Current.CancellationToken));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }
}