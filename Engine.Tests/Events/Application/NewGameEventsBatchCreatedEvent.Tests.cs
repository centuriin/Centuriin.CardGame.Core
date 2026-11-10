using Centuriin.CardGame.Core.Engine.Events.Application;
using Centuriin.CardGame.Core.Engine.Events.Domain;
using Centuriin.CardGame.Core.Engine.Root.State;

using FluentAssertions;

using Moq;

using Xunit;

namespace Centuriin.CardGame.Core.Engine.Tests.Events.Application;

public sealed class NewGameEventsBatchCreatedEventTests
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
        var @event = new NewGameEventsBatchCreatedEvent(gameId, batch.Object);

        // Assert
        @event.GameId.Should().Be(gameId);
        @event.Batch.Should().BeSameAs(batch.Object);
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
            new NewGameEventsBatchCreatedEvent(gameId, null!));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }
}

