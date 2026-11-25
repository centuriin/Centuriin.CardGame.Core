using System.Collections;

using Centuriin.CardGame.Core.Engine.ECS.Components;
using Centuriin.CardGame.Core.Engine.ECS.Entities;
using Centuriin.CardGame.Core.Engine.ECS.Entities.Cards;
using Centuriin.CardGame.Core.Engine.ECS.Entities.Players;
using Centuriin.CardGame.Core.Engine.ECS.Entities.Zones;

using FluentAssertions;

using Xunit;

namespace Centuriin.CardGame.Core.Engine.Tests.ECS.Entities;

public sealed class EntitiesCollectionTests
{
    [Fact(DisplayName = "Can be created as an empty collection.")]
    [Trait("Category", "Unit")]
    public void CanBeCreated()
    {
        // Act
        var entities = new EntitiesCollection();

        // Assert
        entities.IsEmpty.Should().BeTrue();
        entities.Should().BeEmpty();
    }

    [Fact(DisplayName = "Cannot be created without an entity collection.")]
    [Trait("Category", "Unit")]
    public void CanNotBeCreatedWithoutEntities()
    {
        // Act
        var exception = Record.Exception(() =>
            new EntitiesCollection(null!));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = "Can retrieve different entity types with the same identifier.")]
    [Trait("Category", "Unit")]
    public void CanRetrieveEntitiesByTypeAndIdentifier()
    {
        // Arrange
        var card = new Card(new(1));

        var player = new Player(new(1));

        // Act
        var entities = new EntitiesCollection([card, player]);

        // Assert
        entities.Get<Card>(new(1)).Entity.Should().BeSameAs(card);
        entities.Get<Player>(new(1)).Entity.Should().BeSameAs(player);
        ((IReadOnlyEntitiesCollection)entities).Get<Card>(new(1)).Should().BeSameAs(card);
    }

    [Fact(DisplayName = "Can add editable entities and replace an existing entity by type and identifier.")]
    [Trait("Category", "Unit")]
    public void CanAddAndReplaceEditableEntities()
    {
        // Arrange
        var entities = new EntitiesCollection([new Card(new(1)), new Player(new(1))]);

        var replacement = new Card(new(1), [new OwnerComponent(new(2))]);

        // Act
        entities.Add([new EditableEntity<Card>(replacement)]);

        // Assert
        entities.Get<Card>(new(1)).Entity.Should().BeSameAs(replacement);
        entities.Query<Card>().Should().ContainSingle();
        entities.Query<Player>().Should().ContainSingle();
    }

    [Fact(DisplayName = "Cannot add a null entity collection.")]
    [Trait("Category", "Unit")]
    public void CanNotAddNullEntities()
    {
        // Arrange
        var entities = new EntitiesCollection();

        // Act
        var exception = Record.Exception(() =>
            entities.Add((IEnumerable<EntityBase>)null!));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = "Can query only the requested concrete entity type through either view.")]
    [Trait("Category", "Unit")]
    public void CanQueryRequestedEntityType()
    {
        // Arrange
        var card = new Card(new(1));

        var entities = new EntitiesCollection([card, new Player(new(2))]);

        // Act
        var editable = entities.Query<Card>().Select(x => x.Entity).ToArray();
        var readOnly = ((IReadOnlyEntitiesCollection)entities).Query<Card>().ToArray();

        // Assert
        editable.Should().Equal(card);
        readOnly.Should().Equal(card);
        entities.Query<Zone>().Should().BeEmpty();
        ((IReadOnlyEntitiesCollection)entities).Query<Zone>().Should().BeEmpty();
    }

    [Fact(DisplayName = "Can edit an entity retrieved from the collection.")]
    [Trait("Category", "Unit")]
    public void CanEditRetrievedEntity()
    {
        // Arrange
        var entities = new EntitiesCollection([new Card(new(1))]);

        var owner = new OwnerComponent(new(2));

        // Act
        entities.Get<Card>(new(1)).Set(owner);

        // Assert
        ((IReadOnlyEntitiesCollection)entities).Get<Card>(new(1)).Get<OwnerComponent>().Should().BeSameAs(owner);
    }

    [Fact(DisplayName = "Can remove one entity without removing another entity type with the same identifier.")]
    [Trait("Category", "Unit")]
    public void CanRemoveEntity()
    {
        // Arrange
        var card = new Card(new(1));

        var player = new Player(new(1));

        var entities = new EntitiesCollection([card, player]);

        // Act
        entities.Remove(card);

        // Assert
        entities.Query<Card>().Should().BeEmpty();
        entities.Query<Player>().Select(x => x.Entity).Should().Equal(player);
        entities.Should().Equal(player);
        entities.IsEmpty.Should().BeFalse();
    }

    [Fact(DisplayName = "Can report an empty collection after removing its last entity.")]
    [Trait("Category", "Unit")]
    public void CanBecomeEmptyAfterLastRemoval()
    {
        // Arrange
        var card = new Card(new(1));

        var entities = new EntitiesCollection([card]);

        // Act
        entities.Remove(card);

        // Assert
        entities.Should().BeEmpty();
        entities.IsEmpty.Should().BeTrue();
    }

    [Fact(DisplayName = "Cannot retrieve an entity whose type has not been added.")]
    [Trait("Category", "Unit")]
    public void CanNotRetrieveMissingEntityType()
    {
        // Arrange
        var entities = new EntitiesCollection();

        // Act
        var exception = Record.Exception(() =>
            entities.Get<Card>(new(1)));
        var readOnlyException = Record.Exception(() =>
            ((IReadOnlyEntitiesCollection)entities).Get<Card>(new(1)));

        // Assert
        exception.Should().BeOfType<InvalidOperationException>();
        readOnlyException.Should().BeOfType<InvalidOperationException>();
    }

    [Fact(DisplayName = "Cannot retrieve an absent identifier from an existing entity type.")]
    [Trait("Category", "Unit")]
    public void CanNotRetrieveMissingIdentifier()
    {
        // Arrange
        var entities = new EntitiesCollection([new Card(new(1))]);

        // Act
        var exception = Record.Exception(() =>
            entities.Get<Card>(new(2)));

        // Assert
        exception.Should().BeOfType<KeyNotFoundException>();
    }

    [Fact(DisplayName = "Can enumerate all entity types through the non-generic interface.")]
    [Trait("Category", "Unit")]
    public void CanEnumerateEntitiesThroughNonGenericInterface()
    {
        // Arrange
        var card = new Card(new(1));

        var player = new Player(new(2));

        var entities = new EntitiesCollection([card, player]);

        // Act
        var result = ((IEnumerable)entities).Cast<EntityBase>().ToArray();

        // Assert
        result.Should().BeEquivalentTo(new EntityBase[] { card, player });
    }
}
