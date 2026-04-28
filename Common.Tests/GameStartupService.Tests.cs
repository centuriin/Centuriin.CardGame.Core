using Centuriin.CardGame.Core.Common.Factories;
using Centuriin.CardGame.Core.Common.Loaders;
using Centuriin.CardGame.Core.Common.Repositories;
using Centuriin.CardGame.Core.Common.World;

using FluentAssertions;

using Moq;

using Xunit;

namespace Centuriin.CardGame.Core.Common.Tests;

public sealed class GameStartupServiceTests
{
    [Fact]
    public void ConstructorShouldThrowWhenSessionssRepositoryIsNull()
    {
        // Act
        var exception = Record.Exception(() =>
            new GameStartupService(
                null!,
                [],
                Mock.Of<IGameSessionFactory>()));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact]
    public void ConstructorShouldThrowWhenLoadersIsNull()
    {
        // Act
        var exception = Record.Exception(() =>
            new GameStartupService(
                Mock.Of<IGameSessionsRepository>(),
                null!,
                Mock.Of<IGameSessionFactory>()));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact]
    public void ConstructorShouldThrowWhenFactoryIsNull()
    {
        // Act
        var exception = Record.Exception(() =>
            new GameStartupService(
                Mock.Of<IGameSessionsRepository>(),
                [],
                null!));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact]
    public async Task StartupGameAsyncShouldCreateGameLoadDataAndApplyStartedEventAsync()
    {
        // Arrange
        var gameId = new GameId(Guid.NewGuid());
        var setup = new GameSetup(new(1), []);

        var gameState = Mock.Of<IGameState>(MockBehavior.Strict);

        var applyCalls = 0;
        var gameMock = new Mock<IGame>(MockBehavior.Strict);
        gameMock.SetupGet(x => x.Id).Returns(gameId);
        gameMock.SetupGet(x => x.State).Returns(gameState);
        gameMock
            .Setup(x => x.StartAsync(TestContext.Current.CancellationToken))
            .Callback(() => applyCalls++)
            .Returns(Task.CompletedTask);

        var sessionMock = new Mock<IGameSession>(MockBehavior.Strict);
        sessionMock.Setup(x => x.Game)
            .Returns(gameMock.Object);

        var sessionsRepositoryCalls = 0;
        var sessionsRepositoryMock = new Mock<IGameSessionsRepository>(MockBehavior.Strict);
        sessionsRepositoryMock
            .Setup(x => x.AddAsync(sessionMock.Object, TestContext.Current.CancellationToken))
            .Returns(ValueTask.CompletedTask)
            .Callback(() => sessionsRepositoryCalls++);

        var factoryCalls = 0;
        var factoryMock = new Mock<IGameSessionFactory>(MockBehavior.Strict);
        factoryMock
            .Setup(x => x.CreateAsync(setup, TestContext.Current.CancellationToken))
            .Callback(() => factoryCalls++)
            .ReturnsAsync(sessionMock.Object);

        var loaderCalls = 0;
        var loaderMock = new Mock<IGameLoader>(MockBehavior.Strict);
        loaderMock
            .Setup(x => x.LoadAsync(setup, gameState, TestContext.Current.CancellationToken))
            .Callback(() => loaderCalls++)
            .Returns(Task.CompletedTask);

        var service = new GameStartupService(
            sessionsRepositoryMock.Object,
            [loaderMock.Object],
            factoryMock.Object);

        // Act
        var result = await service.StartupGameAsync(setup, TestContext.Current.CancellationToken);

        // Assert
        result.Should().BeSameAs(gameMock.Object);
        factoryCalls.Should().Be(1);
        sessionsRepositoryCalls.Should().Be(1);
        loaderCalls.Should().Be(1);
        applyCalls.Should().Be(1);
    }

    [Fact]
    public async Task StartupGameAsyncShouldThrowWhenSetupIsNullAsync()
    {
        // Arrange
        var service = new GameStartupService(
            Mock.Of<IGameSessionsRepository>(),
            [],
            Mock.Of<IGameSessionFactory>());

        // Act
        var exception = await Record.ExceptionAsync(() =>
            service.StartupGameAsync(null!, TestContext.Current.CancellationToken));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }
}