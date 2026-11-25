using Centuriin.CardGame.Core.Engine.ECS.Components;
using Centuriin.CardGame.Core.Engine.ECS.Entities.Players;
using Centuriin.CardGame.Core.Engine.ECS.Components.Players;
using Centuriin.CardGame.Core.Engine.ECS.Entities;

using FluentAssertions;

using Xunit;

namespace Centuriin.CardGame.Core.Engine.Tests.ECS.Entities.Players;

public sealed class PlayerTests
{
    [Theory(DisplayName = "Can compare player identity independently of component values.")]
    [InlineData(1, 1, true)]
    [InlineData(1, 2, false)]
    [Trait("Category", "Unit")]
    public void CanCompareByIdentifier(int firstId, int secondId, bool expected)
    {
        // Arrange
        var first = new Player(new(firstId), [new OwnerComponent(new(3))]);

        var second = new Player(new(secondId), [new OwnerComponent(new(4))]);

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
        var entity = new Player(new(1));

        // Act
        var unrelatedEquality = entity.Equals(new object());
        var nullEquality = entity.Equals((Player?)null);

        // Assert
        nullEquality.Should().BeFalse();
        unrelatedEquality.Should().BeFalse();
    }

    [Fact(DisplayName = "Can use equal entities as the same hash set identity.")]
    [Trait("Category", "Unit")]
    public void CanUseEntityIdentityInHashSet()
    {
        // Arrange
        var first = new Player(new(1));

        var second = new Player(new(1), [new OwnerComponent(new(2))]);

        // Act
        var entities = new HashSet<Player> { first, second };

        // Assert
        entities.Should().ContainSingle();
        first.GetHashCode().Should().Be(second.GetHashCode());
    }

    [Fact(DisplayName = "Can create independent system player entities with the bank role.")]
    [Trait("Category", "Unit")]
    public void CanCreateSystemPlayer()
    {
        // Act
        var first = Player.System;
        var second = Player.System;

        // Assert
        first.Id.Should().Be(EntityId.Default);
        first.Get<PlayerIdentifierComponent>().PlayerId.Should().Be(PlayerId.System);
        first.Get<PlayerRoleComponent>().Role.Should().Be(PlayerRole.Bank);
        first.Should().NotBeSameAs(second);
    }

}
