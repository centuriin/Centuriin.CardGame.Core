using Centuriin.CardGame.Core.Engine.ECS.Components;
using Centuriin.CardGame.Core.Engine.ECS.Entities;
using Centuriin.CardGame.Core.Engine.ECS.Entities.Cards;

using FluentAssertions;

using Xunit;

namespace Centuriin.CardGame.Core.Engine.Tests;

public sealed class EntityQueryExtensionsTests
{
    [Fact(DisplayName = "Can filter read-only entities by component presence.")]
    [Trait("Category", "Unit")]
    public void CanFilterReadOnlyEntitiesByComponent()
    {
        // Arrange
        var owned = new Card(new(1), [new OwnerComponent(new(2)), new ZoneComponent(new(3))]);

        Card[] cards = [new(new(2)), owned];

        // Act
        var result = cards.WithComponent<Card, OwnerComponent>().ToArray();

        // Assert
        result.Should().Equal(owned);
    }

    [Fact(DisplayName = "Can filter editable entities by component presence.")]
    [Trait("Category", "Unit")]
    public void CanFilterEditableEntitiesByComponent()
    {
        // Arrange
        var owned = new Card(new(1), [new OwnerComponent(new(2)), new ZoneComponent(new(3))]);

        EditableEntity<Card>[] cards = [new(new Card(new(2))), new(owned)];

        // Act
        var result = cards.WithComponent<Card, OwnerComponent>().ToArray();

        // Assert
        result.Select(x => x.Entity).Should().Equal(owned);
    }

    [Fact(DisplayName = "Can filter component values without evaluating missing components.")]
    [Trait("Category", "Unit")]
    public void CanFilterReadOnlyComponentValues()
    {
        // Arrange
        var expected = new Card(new(1), [new OwnerComponent(new(2)), new ZoneComponent(new(3))]);

        Card[] cards = [new(new(4)), new Card(new(5), [new OwnerComponent(new(6)), new ZoneComponent(new(3))]), expected];

        var predicateCalls = 0;

        // Act
        var result = cards.WithComponent((OwnerComponent owner) =>
        {
            predicateCalls++;
            return owner.CurrentOwnerId == new EntityId(2);
        }).ToArray();

        // Assert
        result.Should().Equal(expected);
        predicateCalls.Should().Be(2);
    }

    [Fact(DisplayName = "Can filter editable component values and preserve access to the original entity.")]
    [Trait("Category", "Unit")]
    public void CanFilterEditableComponentValues()
    {
        // Arrange
        var expected = new Card(new(1), [new OwnerComponent(new(2)), new ZoneComponent(new(3))]);

        EditableEntity<Card>[] cards = [new(new Card(new(4))), new(new Card(new(5), [new OwnerComponent(new(6)), new ZoneComponent(new(3))])), new(expected)];

        // Act
        var result = cards.WithComponent((OwnerComponent owner) => owner.CurrentOwnerId == new EntityId(2)).Single();
        result.Set(new ZoneComponent(new(7)));

        // Assert
        result.Entity.Should().BeSameAs(expected);
        expected.Get<ZoneComponent>().CurrentZoneId.Should().Be(new EntityId(7));
    }

    [Fact(DisplayName = "Cannot filter a null read-only entity sequence.")]
    [Trait("Category", "Unit")]
    public void CanNotFilterNullReadOnlyEntities()
    {
        // Act
        var exception = Record.Exception(() =>
            EntityQueryExtensions.WithComponent<Card, OwnerComponent>((IEnumerable<Card>)null!));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = "Cannot filter a null editable entity sequence.")]
    [Trait("Category", "Unit")]
    public void CanNotFilterNullEditableEntities()
    {
        // Act
        var exception = Record.Exception(() =>
            EntityQueryExtensions.WithComponent<Card, OwnerComponent>((IEnumerable<EditableEntity<Card>>)null!));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = "Cannot filter a null read-only entity sequence using a predicate.")]
    [Trait("Category", "Unit")]
    public void CanNotFilterNullReadOnlyEntitiesWithPredicate()
    {
        // Act
        var exception = Record.Exception(() =>
            EntityQueryExtensions.WithComponent<Card, OwnerComponent>(
                (IEnumerable<Card>)null!,
                _ => true));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = "Cannot filter a null editable entity sequence using a predicate.")]
    [Trait("Category", "Unit")]
    public void CanNotFilterNullEditableEntitiesWithPredicate()
    {
        // Act
        var exception = Record.Exception(() =>
            EntityQueryExtensions.WithComponent<Card, OwnerComponent>(
                (IEnumerable<EditableEntity<Card>>)null!,
                _ => true));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = "Cannot filter read-only component values without a predicate.")]
    [Trait("Category", "Unit")]
    public void CanNotFilterReadOnlyEntitiesWithoutPredicate()
    {
        // Arrange
        Card[] cards = [];

        // Act
        var exception = Record.Exception(() =>
            cards.WithComponent<Card, OwnerComponent>(null!));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = "Cannot filter editable component values without a predicate.")]
    [Trait("Category", "Unit")]
    public void CanNotFilterEditableEntitiesWithoutPredicate()
    {
        // Arrange
        EditableEntity<Card>[] cards = [];

        // Act
        var exception = Record.Exception(() =>
            cards.WithComponent<Card, OwnerComponent>(null!));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }
}
