using Centuriin.CardGame.Core.Common.Events.Domain;

using FluentAssertions;

using Xunit;

namespace Centuriin.CardGame.Core.Common.Tests.Events.Domain;

public sealed class GameStartedEventTests
{
    [Fact(DisplayName = "Can be created.")]
    [Trait("Category", "Unit")]
    public void CanBeCreated()
    {
        // Arrange
        var gameId = new GameId(Guid.NewGuid());

        // Act
        var @event = new GameStartedEvent(gameId);

        // Assert
        @event.GameId.Should().Be(gameId);
    }
}

