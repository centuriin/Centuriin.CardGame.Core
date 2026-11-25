using FluentAssertions;

using Moq;

using Xunit;

namespace Centuriin.CardGame.Core.Engine.Tests;

public sealed class GameDescriptionTests
{
    [Fact(DisplayName = "Can be created with the supplied identity, settings, and players.")]
    [Trait("Category", "Unit")]
    public void CanBeCreated()
    {
        // Arrange
        var gameId = new GameId(Guid.NewGuid());

        var settings = Mock.Of<IGameSettings>(MockBehavior.Strict);

        PlayerId[] players = [PlayerId.System, new(Guid.NewGuid())];

        // Act
        var description = new GameDescription(
            gameId,
            new(1),
            settings,
            players);

        // Assert
        description.GameId.Should().Be(gameId);
        description.GameTypeId.Should().Be(new GameTypeId(1));
        description.GameSettings.Should().BeSameAs(settings);
        description.PlayerIds.Should().Equal(players);
    }

    [Fact(DisplayName = "Cannot be created without game settings.")]
    [Trait("Category", "Unit")]
    public void CanNotBeCreatedWithoutSettings()
    {
        // Act
        var exception = Record.Exception(() =>
            new GameDescription(
                new(Guid.NewGuid()),
                new(1),
                null!,
                []));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = "Cannot be created without a player collection.")]
    [Trait("Category", "Unit")]
    public void CanNotBeCreatedWithoutPlayers()
    {
        // Act
        var exception = Record.Exception(() =>
            new GameDescription(
                new(Guid.NewGuid()),
                new(1),
                Mock.Of<IGameSettings>(MockBehavior.Strict),
                null!));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }
}
