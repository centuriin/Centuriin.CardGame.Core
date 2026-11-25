using Centuriin.CardGame.Core.Engine.ECS.Components;
using Centuriin.CardGame.Core.Engine.ECS.Components.Cards;
using Centuriin.CardGame.Core.Engine.ECS.Entities;
using Centuriin.CardGame.Core.Engine.ECS.Entities.Cards;
using Centuriin.CardGame.Core.Engine.ECS.Entities.Zones;
using Centuriin.CardGame.Core.Engine.ECS.Systems;
using Centuriin.CardGame.Core.Engine.Events.Domain;
using Centuriin.CardGame.Core.Engine.Root.State;

using FluentAssertions;

using Moq;

using Xunit;

namespace Centuriin.CardGame.Core.Engine.Tests.ECS.Systems;

public sealed class CardMovementSystemTests
{
    [Fact(DisplayName = "Can move a card to a zone and assign the zone owner while preserving other components.")]
    [Trait("Category", "Unit")]
    public void CanMoveCardToZone()
    {
        // Arrange
        var rank = new RankComponent(CardRank.Ace);

        var card = new Card(new(1), [new OwnerComponent(new(2)), new ZoneComponent(new(3)), rank]);

        var zone = new Zone(new(5), [new OwnerComponent(new(6))]);

        var @event = new CardDealtEvent(new(Guid.NewGuid()), card.Id, zone.Id);

        var entities = new Mock<IEntitiesCollection>(MockBehavior.Strict);
        entities.Setup(x => x.Get<Zone>(zone.Id))
            .Returns(new EditableEntity<Zone>(zone));
        entities.Setup(x => x.Get<Card>(card.Id))
            .Returns(new EditableEntity<Card>(card));

        var state = new Mock<IGameState>(MockBehavior.Strict);
        state.SetupGet(x => x.Entities)
            .Returns(entities.Object);

        var system = new CardMovementSystem();

        // Act
        system.OnEvent(@event, state.Object);

        // Assert
        card.Get<OwnerComponent>().CurrentOwnerId.Should().Be(new EntityId(6));
        card.Get<ZoneComponent>().CurrentZoneId.Should().Be(zone.Id);
        card.Get<RankComponent>().Should().BeSameAs(rank);
    }

    [Fact(DisplayName = "Cannot move a card before locating its destination zone.")]
    [Trait("Category", "Unit")]
    public void CanNotMoveToMissingZone()
    {
        // Arrange
        var @event = new CardDealtEvent(new(Guid.NewGuid()), new(1), new(99));

        var failure = new InvalidOperationException("Zone not found.");

        var entities = new Mock<IEntitiesCollection>(MockBehavior.Strict);
        entities.Setup(x => x.Get<Zone>(@event.NewZoneId))
            .Throws(failure);

        var state = new Mock<IGameState>(MockBehavior.Strict);
        state.SetupGet(x => x.Entities)
            .Returns(entities.Object);

        var system = new CardMovementSystem();

        // Act
        var exception = Record.Exception(() =>
            system.OnEvent(@event, state.Object));

        // Assert
        exception.Should().BeSameAs(failure);
    }

    [Fact(DisplayName = "Cannot move a card without an event.")]
    [Trait("Category", "Unit")]
    public void CanNotHandleWithoutEvent()
    {
        // Act
        var exception = Record.Exception(() =>
            new CardMovementSystem().OnEvent(null!, Mock.Of<IGameState>(MockBehavior.Strict)));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = "Cannot move a card without game state.")]
    [Trait("Category", "Unit")]
    public void CanNotHandleWithoutState()
    {
        // Act
        var exception = Record.Exception(() =>
            new CardMovementSystem().OnEvent(
                new CardDealtEvent(
                    new(Guid.NewGuid()),
                    new(1),
                    new(2)),
                null!));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }
}
