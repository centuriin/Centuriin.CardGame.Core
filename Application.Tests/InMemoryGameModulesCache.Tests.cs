using Centuriin.CardGame.Core.Application.Modulation;
using Centuriin.CardGame.Core.Engine;
using Centuriin.CardGame.Core.Infrastructure.Storage.Abstractions.Repositories;

using FluentAssertions;

using Moq;

using Xunit;

namespace Centuriin.CardGame.Core.Application.Tests;

public sealed class InMemoryGameModulesCacheTests
{
    [Theory(DisplayName = "Can resolve a module using the host's game type mapping.")]
    [InlineData(1)]
    [InlineData(501)]
    [Trait("Category", "Unit")]
    public async Task CanResolveHostGameTypeAsync(int value)
    {
        // Arrange
        var gameTypeId = new GameTypeId(value);
        var token = TestContext.Current.CancellationToken;

        var module = new Mock<IGameModule>(MockBehavior.Strict);
        module.SetupGet(x => x.Key)
            .Returns("fake");

        var gameTypes = new Mock<IGameTypeModulesRepository>(MockBehavior.Strict);
        gameTypes.Setup(x => x.FindModuleKeyAsync(gameTypeId, token))
            .ReturnsAsync("fake");

        var cache = new InMemoryGameModulesCache([module.Object], gameTypes.Object);

        // Act
        var result = await cache.GetModuleAsync(gameTypeId, token);

        // Assert
        result.Should().BeSameAs(module.Object);
    }

    [Fact(DisplayName = "Can map several game types to the same module.")]
    [Trait("Category", "Unit")]
    public async Task CanMapSeveralGameTypesToModuleAsync()
    {
        // Arrange
        var firstType = new GameTypeId(1);
        var secondType = new GameTypeId(42);
        var token = TestContext.Current.CancellationToken;

        var module = new Mock<IGameModule>(MockBehavior.Strict);
        module.SetupGet(x => x.Key)
            .Returns("fake");

        var gameTypes = new Mock<IGameTypeModulesRepository>(MockBehavior.Strict);
        gameTypes.Setup(x => x.FindModuleKeyAsync(firstType, token))
            .ReturnsAsync("fake");
        gameTypes.Setup(x => x.FindModuleKeyAsync(secondType, token))
            .ReturnsAsync("fake");

        var cache = new InMemoryGameModulesCache([module.Object], gameTypes.Object);

        // Act
        var first = await cache.GetModuleAsync(firstType, token);
        var second = await cache.GetModuleAsync(secondType, token);

        // Assert
        first.Should().BeSameAs(module.Object);
        second.Should().BeSameAs(module.Object);
    }

    [Fact(DisplayName = "Can resolve a game type added after the module cache was created.")]
    [Trait("Category", "Unit")]
    public async Task CanResolveNewGameTypeAsync()
    {
        // Arrange
        var firstType = new GameTypeId(1);
        var newType = new GameTypeId(42);
        var token = TestContext.Current.CancellationToken;

        var mappings = new Dictionary<GameTypeId, string> { [firstType] = "fake" };

        var module = new Mock<IGameModule>(MockBehavior.Strict);
        module.SetupGet(x => x.Key)
            .Returns("fake");

        var mappingCalls = 0;
        var gameTypes = new Mock<IGameTypeModulesRepository>(MockBehavior.Strict);
        gameTypes.Setup(x => x.FindModuleKeyAsync(It.IsAny<GameTypeId>(), token))
            .Callback(() => mappingCalls++)
            .Returns<GameTypeId, CancellationToken>((id, _) =>
                Task.FromResult(mappings.GetValueOrDefault(id)));

        var cache = new InMemoryGameModulesCache([module.Object], gameTypes.Object);
        _ = await cache.GetModuleAsync(firstType, token);

        // Act
        mappings.Add(newType, "fake");
        var result = await cache.GetModuleAsync(newType, token);

        // Assert
        result.Should().BeSameAs(module.Object);
        mappingCalls.Should().Be(2);
    }

    [Fact(DisplayName = "Cannot resolve an unregistered game type.")]
    [Trait("Category", "Unit")]
    public async Task CanNotResolveUnregisteredGameTypeAsync()
    {
        // Arrange
        var gameTypeId = new GameTypeId(42);
        var token = TestContext.Current.CancellationToken;

        var gameTypes = new Mock<IGameTypeModulesRepository>(MockBehavior.Strict);
        gameTypes.Setup(x => x.FindModuleKeyAsync(gameTypeId, token))
            .ReturnsAsync((string?)null);

        var cache = new InMemoryGameModulesCache([], gameTypes.Object);

        // Act
        var exception = await Record.ExceptionAsync(async () =>
            await cache.GetModuleAsync(gameTypeId, token));

        // Assert
        exception.Should().BeOfType<InvalidOperationException>()
            .Which.Message.Should().Contain("not registered");
    }

    [Fact(DisplayName = "Cannot resolve a game type whose module is not installed.")]
    [Trait("Category", "Unit")]
    public async Task CanNotResolveUninstalledModuleAsync()
    {
        // Arrange
        var gameTypeId = new GameTypeId(42);
        var token = TestContext.Current.CancellationToken;

        var gameTypes = new Mock<IGameTypeModulesRepository>(MockBehavior.Strict);
        gameTypes.Setup(x => x.FindModuleKeyAsync(gameTypeId, token))
            .ReturnsAsync("missing");

        var cache = new InMemoryGameModulesCache([], gameTypes.Object);

        // Act
        var exception = await Record.ExceptionAsync(async () =>
            await cache.GetModuleAsync(gameTypeId, token));

        // Assert
        exception.Should().BeOfType<InvalidOperationException>()
            .Which.Message.Should().Contain("missing").And.Contain("not installed");
    }
}
