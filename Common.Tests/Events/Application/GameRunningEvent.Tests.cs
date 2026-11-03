using Centuriin.CardGame.Core.Common.Events.Application;
using Centuriin.CardGame.Core.Common.Events.Domain;
using Centuriin.CardGame.Core.Common.Root.State;

using FluentAssertions;

using Moq;

using Xunit;

namespace Centuriin.CardGame.Core.Common.Tests.Events.Application;

public sealed class GameRunningEventTests
{
    [Fact(DisplayName = "Can be created with the game and batch version.")]
    [Trait("Category", "Unit")]
    public void CanBeCreated()
    {
        // Arrange
        var gameId = new GameId(Guid.NewGuid());
        var version = new GameVersion(1);

        var batch = new Mock<IGameEventsBatch>(MockBehavior.Strict);
        batch.Setup(x => x.CurrentGameVersion)
            .Returns(version);

        // Act
        var @event = new GameRunningEvent(gameId, batch.Object);

        // Assert
        @event.GameId.Should().Be(gameId);
        @event.InitialEventsBatch.Should().BeSameAs(batch.Object);
        @event.GameVersion.Should().Be(version);
    }

    [Fact(DisplayName = "Cannot be created without a batch.")]
    [Trait("Category", "Unit")]
    public void CanNotBeCreatedWithoutBatch()
    {
        // Arrange
        var gameId = new GameId(Guid.NewGuid());

        // Act
        var exception = Record.Exception(() =>
            new GameRunningEvent(gameId, null!));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }
}

