using Centuriin.CardGame.Core.Common;
using Centuriin.CardGame.Core.Common.Entities.Cards;
using Centuriin.CardGame.Core.Common.Entities.Zones;

using FluentAssertions;

using Xunit;

namespace Centuriin.CardGame.Core.Tests;

public sealed class GameStateTests
{
    [Fact]
    public void AddEntityShouldStoreEntityAndAllowRetrievalViaQuery()
    {
        // Arrange
        var cardId = new CardId(1);
        var card = new Card(cardId);

        var gameState = new GameState(new(Guid.NewGuid()));

        // Act
        gameState.AddEntity<Card, CardId>(card);
        var result = gameState.Query<Card>();

        // Assert
        result.Should().ContainSingle()
            .Which.Should().Be(card);
    }

    [Fact]
    public void AddEntityShouldOverwriteExistingEntityWithSameId()
    {
        // Arrange
        var cardId = new CardId(1);
        var initialCard = new Card(cardId);
        var updatedCard = new Card(cardId);

        var gameState = new GameState(new(Guid.NewGuid()));

        // Act
        gameState.AddEntity<Card, CardId>(initialCard);
        gameState.AddEntity<Card, CardId>(updatedCard);
        var result = gameState.Query<Card>();

        // Assert
        result.Should().ContainSingle()
            .Which.Should().Be(updatedCard);
    }

    [Fact]
    public void QueryShouldReturnEmptyCollectionWhenNoEntitiesAdded()
    {
        // Arrange
        var gameState = new GameState(new(Guid.NewGuid()));

        // Act
        var result = gameState.Query<Card>();

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void GameStateShouldIsolateDifferentEntityTypes()
    {
        // Arrange
        var card = new Card(new CardId(1));
        var zone = new Zone(new ZoneId(10));

        var gameState = new GameState(new(Guid.NewGuid()));

        // Act
        gameState.AddEntity<Card, CardId>(card);
        gameState.AddEntity<Zone, ZoneId>(zone);

        var cards = gameState.Query<Card>();
        var zones = gameState.Query<Zone>();

        // Assert
        cards.Should().ContainSingle().Which.Should().Be(card);
        zones.Should().ContainSingle().Which.Should().Be(zone);
    }

    [Fact]
    public void QueryShouldReturnMultipleEntitiesOfSameType()
    {
        // Arrange
        var card1 = new Card(new CardId(1));
        var card2 = new Card(new CardId(2));

        var gameState = new GameState(new(Guid.NewGuid()));

        // Act
        gameState.AddEntity<Card, CardId>(card1);
        gameState.AddEntity<Card, CardId>(card2);
        var result = gameState.Query<Card>();

        // Assert
        result.Should().HaveCount(2)
            .And.Contain([card1, card2]);
    }

    [Fact]
    public void GetShouldReturnCorrectEntityWhenIdExists()
    {
        // Arrange
        var cardId = new CardId(1);
        var card = new Card(cardId);

        var gameState = new GameState(new(Guid.NewGuid()));

        gameState.AddEntity<Card, CardId>(card);

        // Act
        var result = gameState.Get<Card, CardId>(cardId);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(cardId);
        result.Should().BeSameAs(card);
    }

    [Fact]
    public void GetShouldThrowInvalidOperationExceptionWhenEntityTableDoesNotExist()
    {
        // Arrange
        var gameState = new GameState(new(Guid.NewGuid()));
        var anyId = new CardId(1);

        // Act
        var exception = Record.Exception(() => gameState.Get<Card, CardId>(anyId));

        // Assert
        exception.Should().BeOfType<InvalidOperationException>();
    }
}
