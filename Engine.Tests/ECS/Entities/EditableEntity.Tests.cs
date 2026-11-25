using Centuriin.CardGame.Core.Engine.ECS.Components;
using Centuriin.CardGame.Core.Engine.ECS.Entities;
using Centuriin.CardGame.Core.Engine.ECS.Entities.Cards;

using FluentAssertions;

using Xunit;

namespace Centuriin.CardGame.Core.Engine.Tests.ECS.Entities;

public sealed class EditableEntityTests
{
    [Fact(DisplayName = "Can be created for an existing entity.")]
    [Trait("Category", "Unit")]
    public void CanBeCreated()
    {
        // Arrange
        var card = new Card(new(1));

        // Act
        var editable = new EditableEntity<Card>(card);

        // Assert
        editable.Id.Should().Be(card.Id);
        editable.Entity.Should().BeSameAs(card);
        ((Card)editable).Should().BeSameAs(card);
    }

    [Fact(DisplayName = "Cannot be created without an entity.")]
    [Trait("Category", "Unit")]
    public void CanNotBeCreatedWithoutEntity()
    {
        // Act
        var exception = Record.Exception(() =>
            new EditableEntity<Card>(null!));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = "Can chain component edits on the underlying entity.")]
    [Trait("Category", "Unit")]
    public void CanChainComponentEdits()
    {
        // Arrange
        var card = new Card(new(1));

        var editable = new EditableEntity<Card>(card);

        var owner = new OwnerComponent(new(2));

        var zone = new ZoneComponent(new(3));

        // Act
        var result = editable.Set(owner, zone).Remove<OwnerComponent>();

        // Assert
        result.Entity.Should().BeSameAs(card);
        editable.Has<OwnerComponent>().Should().BeFalse();
        editable.Get<ZoneComponent>().Should().BeSameAs(zone);
        card.Get<ZoneComponent>().Should().BeSameAs(zone);
    }

    [Fact(DisplayName = "Cannot set a null component collection through an editable entity.")]
    [Trait("Category", "Unit")]
    public void CanNotSetNullComponents()
    {
        // Arrange
        var editable = new EditableEntity<Card>(new Card(new(1)));

        // Act
        var exception = Record.Exception(() =>
            editable.Set(null!));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }
}
