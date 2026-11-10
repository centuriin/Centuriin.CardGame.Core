using Centuriin.CardGame.Core.Engine;
using Centuriin.CardGame.Core.Engine.Root.State;

using FluentAssertions;

using Xunit;

namespace Centuriin.CardGame.Core.Application.Tests.Events;

public sealed class GameRestoredEventTests
{
    [Fact(DisplayName = "Can be created.")]
    [Trait("Category", "Unit")]
    public void CanBeCreated()
    {
        // Arrange
        var gameId = new GameId(Guid.NewGuid());
        var gameVersion = new GameVersion(7);

        // Act
        var @event = new GameRestoredEvent(gameId, gameVersion);

        // Assert
        @event.GameId.Should().Be(gameId);
        @event.GameVersion.Should().Be(gameVersion);
    }
}

