using Centuriin.CardGame.Core.Engine.ECS.Components;
using Centuriin.CardGame.Core.Engine.ECS.Components.Zones;
using Centuriin.CardGame.Core.Engine.ECS.Entities;
using Centuriin.CardGame.Core.Engine.ECS.Entities.Cards;
using Centuriin.CardGame.Core.Engine.ECS.Entities.Zones;
using Centuriin.CardGame.Core.Engine.ECS.Systems;
using Centuriin.CardGame.Core.Engine.Events;
using Centuriin.CardGame.Core.Engine.Events.Domain;
using Centuriin.CardGame.Core.Engine.Root.State;

using FluentAssertions;

using Moq;

using Xunit;

namespace Centuriin.CardGame.Core.Engine.Tests.ECS.Systems;

public sealed class DealerSystemTests
{
    [Fact(DisplayName = "Can deal cards from each zone owner's deck and fall back to the shared deck.")]
    [Trait("Category", "Unit")]
    public void CanDealFromOwnerAndSharedDecks()
    {
        // Arrange
        var gameId = new GameId(Guid.NewGuid());

        Card[] cards = [
            new(new(1), [new OwnerComponent(new(10))]),
            new(new(2), [new OwnerComponent(new(10))]),
            new(new(3), [new OwnerComponent(EntityId.Default)]),
            new(new(4), [new OwnerComponent(EntityId.Default)])
        ];

        Zone[] zones = [
            new(new(20), [new OwnerComponent(new(10)), new HasPrimaryCards(2)]),
            new(new(21), [new OwnerComponent(new(11)), new HasPrimaryCards(2)])
        ];

        var events = new List<CardDealtEvent>();

        var entities = new Mock<IReadOnlyEntitiesCollection>(MockBehavior.Strict);
        entities.Setup(x => x.Query<Card>())
            .Returns(cards);
        entities.Setup(x => x.Query<Zone>())
            .Returns(zones);

        var state = new Mock<IReadOnlyGameState>(MockBehavior.Strict);
        state.SetupGet(x => x.Entities)
            .Returns(entities.Object);

        var writer = new Mock<IGameEventBusWriter>(MockBehavior.Strict);
        writer.Setup(x => x.Write(It.IsAny<CardDealtEvent>()))
            .Callback<IGameEvent>(e => events.Add((CardDealtEvent)e));

        var system = new DealerSystem();

        // Act
        system.OnEvent(new GameStartedEvent(gameId), state.Object, writer.Object);

        // Assert
        events.Should().HaveCount(4);
        events.Should().OnlyContain(e => e.GameId == gameId);
        events.Where(e => e.NewZoneId == new EntityId(20)).Select(e => e.CardId)
            .Should().BeEquivalentTo(new EntityId[] { new(1), new(2) });
        events.Where(e => e.NewZoneId == new EntityId(21)).Select(e => e.CardId)
            .Should().BeEquivalentTo(new EntityId[] { new(3), new(4) });
        cards[2].Get<OwnerComponent>().CurrentOwnerId.Should().Be(EntityId.Default);
    }

    [Fact(DisplayName = "Can distribute a shared deck to several zones without dealing a card twice.")]
    [Trait("Category", "Unit")]
    public void CanDealSharedDeckWithoutDuplicates()
    {
        // Arrange
        Card[] cards = [
            new(new(1), [new OwnerComponent(EntityId.Default)]),
            new(new(2), [new OwnerComponent(EntityId.Default)]),
            new(new(3), [new OwnerComponent(EntityId.Default)]),
            new(new(4), [new OwnerComponent(EntityId.Default)]),
            new(new(5), [new OwnerComponent(EntityId.Default)]),
            new(new(6), [new OwnerComponent(EntityId.Default)])
        ];

        Zone[] zones = [
            new(new(20), [new OwnerComponent(new(10)), new HasPrimaryCards(2)]),
            new(new(21), [new OwnerComponent(new(11)), new HasPrimaryCards(2)])
        ];

        var events = new List<CardDealtEvent>();

        var entities = new Mock<IReadOnlyEntitiesCollection>(MockBehavior.Strict);
        entities.Setup(x => x.Query<Card>())
            .Returns(cards);
        entities.Setup(x => x.Query<Zone>())
            .Returns(zones);

        var state = new Mock<IReadOnlyGameState>(MockBehavior.Strict);
        state.SetupGet(x => x.Entities)
            .Returns(entities.Object);

        var writer = new Mock<IGameEventBusWriter>(MockBehavior.Strict);
        writer.Setup(x => x.Write(It.IsAny<CardDealtEvent>()))
            .Callback<IGameEvent>(e => events.Add((CardDealtEvent)e));

        var system = new DealerSystem();

        // Act
        system.OnEvent(new GameStartedEvent(new(Guid.NewGuid())), state.Object, writer.Object);

        // Assert
        events.Select(e => e.CardId).Should().OnlyHaveUniqueItems().And.HaveCount(4);
        events.Should().OnlyContain(e => e.CardId.Value >= 1 && e.CardId.Value <= 6);
        events.Count(e => e.NewZoneId == new EntityId(20)).Should().Be(2);
        events.Count(e => e.NewZoneId == new EntityId(21)).Should().Be(2);
    }

    [Fact(DisplayName = "Can ignore zones that do not request initial cards.")]
    [Trait("Category", "Unit")]
    public void CanIgnoreZonesWithoutInitialCards()
    {
        // Arrange
        Zone[] zones = [new(new(1), [new OwnerComponent(new(2))])];

        var entities = new Mock<IReadOnlyEntitiesCollection>(MockBehavior.Strict);
        entities.Setup(x => x.Query<Card>())
            .Returns([]);
        entities.Setup(x => x.Query<Zone>())
            .Returns(zones);

        var state = new Mock<IReadOnlyGameState>(MockBehavior.Strict);
        state.SetupGet(x => x.Entities)
            .Returns(entities.Object);

        var system = new DealerSystem();

        // Act
        var exception = Record.Exception(() =>
            system.OnEvent(
                new GameStartedEvent(new(Guid.NewGuid())),
                state.Object,
                Mock.Of<IGameEventBusWriter>(MockBehavior.Strict)));

        // Assert
        exception.Should().BeNull();
    }

    [Fact(DisplayName = "Cannot deal more cards than are available in a deck.")]
    [Trait("Category", "Unit")]
    public void CanNotDealMoreThanAvailable()
    {
        // Arrange
        Card[] cards = [new(new(1), [new OwnerComponent(EntityId.Default)])];

        Zone[] zones = [new(new(20), [new OwnerComponent(new(10)), new HasPrimaryCards(2)])];

        var entities = new Mock<IReadOnlyEntitiesCollection>(MockBehavior.Strict);
        entities.Setup(x => x.Query<Card>())
            .Returns(cards);
        entities.Setup(x => x.Query<Zone>())
            .Returns(zones);

        var state = new Mock<IReadOnlyGameState>(MockBehavior.Strict);
        state.SetupGet(x => x.Entities)
            .Returns(entities.Object);

        var writer = new Mock<IGameEventBusWriter>(MockBehavior.Strict);
        writer.Setup(x => x.Write(It.IsAny<CardDealtEvent>()));

        var system = new DealerSystem();

        // Act
        var exception = Record.Exception(() =>
            system.OnEvent(
                new GameStartedEvent(new(Guid.NewGuid())),
                state.Object,
                writer.Object));

        // Assert
        exception.Should().BeOfType<InvalidOperationException>();
    }

    [Fact(DisplayName = "Cannot deal cards without an event.")]
    [Trait("Category", "Unit")]
    public void CanNotHandleWithoutEvent()
    {
        // Act
        var exception = Record.Exception(() =>
            new DealerSystem().OnEvent(
                null!,
                Mock.Of<IReadOnlyGameState>(MockBehavior.Strict),
                Mock.Of<IGameEventBusWriter>(MockBehavior.Strict)));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = "Cannot deal cards without game state.")]
    [Trait("Category", "Unit")]
    public void CanNotHandleWithoutState()
    {
        // Act
        var exception = Record.Exception(() =>
            new DealerSystem().OnEvent(
                new GameStartedEvent(new(Guid.NewGuid())),
                null!,
                Mock.Of<IGameEventBusWriter>(MockBehavior.Strict)));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = "Cannot deal cards without an event writer.")]
    [Trait("Category", "Unit")]
    public void CanNotHandleWithoutWriter()
    {
        // Act
        var exception = Record.Exception(() =>
            new DealerSystem().OnEvent(
                new GameStartedEvent(new(Guid.NewGuid())),
                Mock.Of<IReadOnlyGameState>(MockBehavior.Strict),
                null!));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }
}
