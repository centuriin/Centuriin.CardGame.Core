using Centuriin.CardGame.Core.Engine.ECS.Components;
using Centuriin.CardGame.Core.Engine.ECS.Entities.Cards;

using FluentAssertions;

using Xunit;

namespace Centuriin.CardGame.Core.Engine.Tests.ECS.Entities.Cards;

public sealed class CardTests
{
    [Theory(DisplayName = "Can compare card identity independently of component values.")]
    [InlineData(1, 1, true)]
    [InlineData(1, 2, false)]
    [Trait("Category", "Unit")]
    public void CanCompareByIdentifier(int firstId, int secondId, bool expected)
    {
        // Arrange
        var first = new Card(new(firstId), [new OwnerComponent(new(3))]);

        var second = new Card(new(secondId), [new OwnerComponent(new(4))]);

        // Act
        var typedEquality = first.Equals(second);
        var objectEquality = first.Equals((object)second);

        // Assert
        typedEquality.Should().Be(expected);
        objectEquality.Should().Be(expected);
    }

    [Fact(DisplayName = "Cannot compare equal to null or an unrelated object.")]
    [Trait("Category", "Unit")]
    public void CanNotEqualNullOrUnrelatedObject()
    {
        // Arrange
        var entity = new Card(new(1));

        // Act
        var unrelatedEquality = entity.Equals(new object());
        var nullEquality = entity.Equals((Card?)null);

        // Assert
        nullEquality.Should().BeFalse();
        unrelatedEquality.Should().BeFalse();
    }

    [Fact(DisplayName = "Can use equal entities as the same hash set identity.")]
    [Trait("Category", "Unit")]
    public void CanUseEntityIdentityInHashSet()
    {
        // Arrange
        var first = new Card(new(1));

        var second = new Card(new(1), [new OwnerComponent(new(2))]);

        // Act
        var entities = new HashSet<Card> { first, second };

        // Assert
        entities.Should().ContainSingle();
        first.GetHashCode().Should().Be(second.GetHashCode());
    }

}
