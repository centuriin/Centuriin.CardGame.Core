using System.Collections;

using Centuriin.CardGame.Core.Engine.ECS.Components;

using FluentAssertions;

using Xunit;

namespace Centuriin.CardGame.Core.Engine.Tests.ECS.Components;

public sealed class ComponentsCollectionTests
{
    [Fact(DisplayName = "Can be created as an empty collection.")]
    [Trait("Category", "Unit")]
    public void CanBeCreated()
    {
        // Act
        var components = new ComponentsCollection();

        // Assert
        components.IsEmpty.Should().BeTrue();
        components.Should().BeEmpty();
    }

    [Fact(DisplayName = "Cannot be created without a component collection.")]
    [Trait("Category", "Unit")]
    public void CanNotBeCreatedWithoutComponents()
    {
        // Act
        var exception = Record.Exception(() =>
            new ComponentsCollection(null!));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = "Can retrieve initial components by their concrete types.")]
    [Trait("Category", "Unit")]
    public void CanRetrieveInitialComponents()
    {
        // Arrange
        var owner = new OwnerComponent(new(1));

        var zone = new ZoneComponent(new(2));

        // Act
        var components = new ComponentsCollection([owner, zone]);

        // Assert
        components.Get<OwnerComponent>().Should().BeSameAs(owner);
        components.Get<ZoneComponent>().Should().BeSameAs(zone);
        components.Has<TemplateComponent>().Should().BeFalse();
    }

    [Fact(DisplayName = "Cannot initialize two components of the same concrete type.")]
    [Trait("Category", "Unit")]
    public void CanNotBeCreatedWithDuplicateComponentTypes()
    {
        // Act
        var exception = Record.Exception(() =>
            new ComponentsCollection([new OwnerComponent(new(1)), new OwnerComponent(new(2))]));

        // Assert
        exception.Should().BeOfType<ArgumentException>();
    }

    [Fact(DisplayName = "Can replace one component without changing other component types.")]
    [Trait("Category", "Unit")]
    public void CanReplaceComponents()
    {
        // Arrange
        var zone = new ZoneComponent(new(2));

        var components = new ComponentsCollection([new OwnerComponent(new(1)), zone]);

        var replacement = new OwnerComponent(new(3));

        // Act
        components.Set(replacement);

        // Assert
        components.Get<OwnerComponent>().Should().BeSameAs(replacement);
        components.Get<ZoneComponent>().Should().BeSameAs(zone);
        components.Should().HaveCount(2);
    }

    [Fact(DisplayName = "Can add several component types at once.")]
    [Trait("Category", "Unit")]
    public void CanSetComponents()
    {
        // Arrange
        var components = new ComponentsCollection();

        // Act
        components.Set(new OwnerComponent(new(1)), new ZoneComponent(new(2)));

        // Assert
        components.IsEmpty.Should().BeFalse();
        components.Has<OwnerComponent>().Should().BeTrue();
        components.Has<ZoneComponent>().Should().BeTrue();
    }

    [Fact(DisplayName = "Cannot set a null component collection.")]
    [Trait("Category", "Unit")]
    public void CanNotSetNullComponents()
    {
        // Arrange
        var components = new ComponentsCollection();

        // Act
        var exception = Record.Exception(() =>
            components.Set(null!));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = "Cannot retrieve a missing component.")]
    [Trait("Category", "Unit")]
    public void CanNotRetrieveMissingComponent()
    {
        // Arrange
        var components = new ComponentsCollection();

        // Act
        var exception = Record.Exception(() =>
            components.Get<OwnerComponent>());

        // Assert
        exception.Should().BeOfType<InvalidOperationException>();
    }

    [Fact(DisplayName = "Can remove the last component and become empty.")]
    [Trait("Category", "Unit")]
    public void CanRemoveLastComponent()
    {
        // Arrange
        var components = new ComponentsCollection([new OwnerComponent(new(1))]);

        // Act
        components.Remove<OwnerComponent>();

        // Assert
        components.Has<OwnerComponent>().Should().BeFalse();
        components.IsEmpty.Should().BeTrue();
        components.Should().BeEmpty();
    }

    [Fact(DisplayName = "Can remove an absent component without changing existing components.")]
    [Trait("Category", "Unit")]
    public void CanRemoveAbsentComponent()
    {
        // Arrange
        var owner = new OwnerComponent(new(1));

        var components = new ComponentsCollection([owner]);

        // Act
        components.Remove<ZoneComponent>();

        // Assert
        components.Should().Equal(owner);
    }

    [Fact(DisplayName = "Can enumerate components through the non-generic interface.")]
    [Trait("Category", "Unit")]
    public void CanEnumerateComponentsThroughNonGenericInterface()
    {
        // Arrange
        var owner = new OwnerComponent(new(1));

        var components = new ComponentsCollection([owner]);

        // Act
        var enumerator = ((IEnumerable)components).GetEnumerator();
        var hasFirst = enumerator.MoveNext();
        var first = enumerator.Current;
        var hasSecond = enumerator.MoveNext();

        // Assert
        hasFirst.Should().BeTrue();
        first.Should().BeSameAs(owner);
        hasSecond.Should().BeFalse();
    }
}
