using Centuriin.CardGame.Core.Common.Events.Application;
using Centuriin.CardGame.Core.Common.Root.State;

using FluentAssertions;

using Xunit;

namespace Centuriin.CardGame.Core.Common.Tests.Events.Application;

public sealed class GameAbortedEventTests
{
    [Fact(DisplayName = "Can be created.")]
    [Trait("Category", "Unit")]
    public void CanBeCreated()
    {
        // Arrange
        var gameId = new GameId(Guid.NewGuid());
        var gameVersion = new GameVersion(7);

        // Act
        var @event = new GameAbortedEvent(gameId, gameVersion);

        // Assert
        @event.GameId.Should().Be(gameId);
        @event.GameVersion.Should().Be(gameVersion);
    }
}

