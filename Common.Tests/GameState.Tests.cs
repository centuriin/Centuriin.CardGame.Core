using Centuriin.CardGame.Core.Common.Entities;
using Centuriin.CardGame.Core.Common.Entities.Cards;
using Centuriin.CardGame.Core.Common.Entities.Zones;
using Centuriin.CardGame.Core.Common.World;

using FluentAssertions;

using Moq;

using Xunit;

namespace Centuriin.CardGame.Core.Common.Tests;

public sealed class GameStateTests
{
    [Fact]
    public void AddEntityShouldStoreEntityAndAllowRetrievalViaQuery()
    {
        // Arrange
        var cardId = new EntityId(1);
        var card = new Card(cardId);

        var gameState = new GameState(Mock.Of<ITurnAutomat>(MockBehavior.Strict));

        // Act
        gameState.AddEntity(card);
        var result = gameState.Query<Card>();

        // Assert
        result.Should().ContainSingle()
            .Which.Should().Be(card);
    }

    [Fact]
    public void AddEntityShouldOverwriteExistingEntityWithSameId()
    {
        // Arrange
        var cardId = new EntityId(1);
        var initialCard = new Card(cardId);
        var updatedCard = new Card(cardId);

        var gameState = new GameState(Mock.Of<ITurnAutomat>(MockBehavior.Strict));

        // Act
        gameState.AddEntity(initialCard);
        gameState.AddEntity(updatedCard);
        var result = gameState.Query<Card>();

        // Assert
        result.Should().ContainSingle()
            .Which.Should().Be(updatedCard);
    }

    [Fact]
    public void QueryShouldReturnEmptyCollectionWhenNoEntitiesAdded()
    {
        // Arrange
        var gameState = new GameState(Mock.Of<ITurnAutomat>(MockBehavior.Strict));

        // Act
        var result = gameState.Query<Card>();

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void GameStateShouldIsolateDifferentEntityTypes()
    {
        // Arrange
        var card = new Card(new(1));
        var zone = new Zone(new(10));

        var gameState = new GameState(Mock.Of<ITurnAutomat>(MockBehavior.Strict));

        // Act
        gameState.AddEntity(card);
        gameState.AddEntity(zone);

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
        var card1 = new Card(new(1));
        var card2 = new Card(new(2));

        var gameState = new GameState(Mock.Of<ITurnAutomat>(MockBehavior.Strict));

        // Act
        gameState.AddEntity(card1);
        gameState.AddEntity(card2);
        var result = gameState.Query<Card>();

        // Assert
        result.Should().HaveCount(2)
            .And.Contain([card1, card2]);
    }

    [Fact]
    public void GetShouldReturnCorrectEntityWhenIdExists()
    {
        // Arrange
        var cardId = new EntityId(1);
        var card = new Card(cardId);

        var gameState = new GameState(Mock.Of<ITurnAutomat>(MockBehavior.Strict));

        gameState.AddEntity(card);

        // Act
        var result = gameState.Get<Card>(cardId);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(cardId);
        result.Should().BeSameAs(card);
    }

    [Fact]
    public void GetShouldThrowInvalidOperationExceptionWhenEntityTableDoesNotExist()
    {
        // Arrange
        var gameState = new GameState(Mock.Of<ITurnAutomat>(MockBehavior.Strict));
        var anyId = new EntityId(1);

        // Act
        var exception = Record.Exception(() => gameState.Get<Card>(anyId));

        // Assert
        exception.Should().BeOfType<InvalidOperationException>();
    }
}
