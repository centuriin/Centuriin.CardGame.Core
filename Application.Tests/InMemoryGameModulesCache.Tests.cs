using Centuriin.CardGame.Core.Application.Modulation;
using Centuriin.CardGame.Core.Engine;

using FluentAssertions;

using Moq;

using Xunit;

namespace Centuriin.CardGame.Core.Application.Tests;

public sealed class InMemoryGameModulesCacheTests
{
    [Fact(DisplayName = "Cannot create a module cache without game type mappings.")]
    [Trait("Category", "Unit")]
    public void CanNotCreateWithoutModules()
    {
        // Act
        var exception = Record.Exception(() =>
            new InMemoryGameModulesCache(null!));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>()
            .Which.ParamName.Should().Be("modules");
    }

    [Theory(DisplayName = "Can resolve a module using the host's game type mapping.")]
    [InlineData(1)]
    [InlineData(501)]
    [Trait("Category", "Unit")]
    public async Task CanResolveHostGameTypeAsync(int value)
    {
        // Arrange
        var gameTypeId = new GameTypeId(value);
        var token = TestContext.Current.CancellationToken;

        var module = Mock.Of<IGameModule>(MockBehavior.Strict);

        var cache = new InMemoryGameModulesCache([new(gameTypeId, module)]);

        // Act
        var result = await cache.GetModuleAsync(gameTypeId, token);

        // Assert
        result.Should().BeSameAs(module);
    }

    [Fact(DisplayName = "Can map several game types to the same module.")]
    [Trait("Category", "Unit")]
    public async Task CanMapSeveralGameTypesToModuleAsync()
    {
        // Arrange
        var firstType = new GameTypeId(1);
        var secondType = new GameTypeId(42);
        var token = TestContext.Current.CancellationToken;

        var module = Mock.Of<IGameModule>(MockBehavior.Strict);

        var cache = new InMemoryGameModulesCache(
            [new(firstType, module), new(secondType, module)]);

        // Act
        var first = await cache.GetModuleAsync(firstType, token);
        var second = await cache.GetModuleAsync(secondType, token);

        // Assert
        first.Should().BeSameAs(module);
        second.Should().BeSameAs(module);
    }

    [Fact(DisplayName = "Can resolve different modules for different game types.")]
    [Trait("Category", "Unit")]
    public async Task CanResolveDifferentModulesAsync()
    {
        // Arrange
        var firstType = new GameTypeId(1);
        var secondType = new GameTypeId(42);
        var token = TestContext.Current.CancellationToken;

        var firstModule = Mock.Of<IGameModule>(MockBehavior.Strict);

        var secondModule = Mock.Of<IGameModule>(MockBehavior.Strict);

        var cache = new InMemoryGameModulesCache(
            [new(firstType, firstModule), new(secondType, secondModule)]);

        // Act
        var first = await cache.GetModuleAsync(firstType, token);
        var second = await cache.GetModuleAsync(secondType, token);

        // Assert
        first.Should().BeSameAs(firstModule);
        second.Should().BeSameAs(secondModule);
    }

    [Fact(DisplayName = "Cannot resolve an unregistered game type.")]
    [Trait("Category", "Unit")]
    public async Task CanNotResolveUnregisteredGameTypeAsync()
    {
        // Arrange
        var gameTypeId = new GameTypeId(42);
        var token = TestContext.Current.CancellationToken;

        var cache = new InMemoryGameModulesCache([]);

        // Act
        var exception = await Record.ExceptionAsync(async () =>
            await cache.GetModuleAsync(gameTypeId, token));

        // Assert
        exception.Should().BeOfType<InvalidOperationException>()
            .Which.Message.Should().Contain(gameTypeId.Value.ToString()).And.Contain("not found");
    }

    [Fact(DisplayName = "Cannot resolve a module when cancellation is requested.")]
    [Trait("Category", "Unit")]
    public async Task CanNotResolveWhenCancelledAsync()
    {
        // Arrange
        var gameTypeId = new GameTypeId(1);

        var module = Mock.Of<IGameModule>(MockBehavior.Strict);

        using var cancellation = CancellationTokenSource.CreateLinkedTokenSource(
            TestContext.Current.CancellationToken);
        await cancellation.CancelAsync();

        var cache = new InMemoryGameModulesCache([new(gameTypeId, module)]);

        // Act
        var exception = await Record.ExceptionAsync(async () =>
            await cache.GetModuleAsync(gameTypeId, cancellation.Token));

        // Assert
        exception.Should().BeOfType<OperationCanceledException>()
            .Which.CancellationToken.Should().Be(cancellation.Token);
    }
}
