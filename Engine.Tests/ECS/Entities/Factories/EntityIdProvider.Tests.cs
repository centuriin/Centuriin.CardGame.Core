using Centuriin.CardGame.Core.Engine.ECS.Entities;
using Centuriin.CardGame.Core.Engine.ECS.Entities.Factories;

using FluentAssertions;

using Xunit;

namespace Centuriin.CardGame.Core.Engine.Tests.ECS.Entities.Factories;

public sealed class EntityIdProviderTests
{
    [Fact(DisplayName = "Can allocate distinct sequential entity identifiers starting after the system identifier.")]
    [Trait("Category", "Unit")]
    public void CanAllocateSequentialIdentifiers()
    {
        // Arrange
        var provider = new EntityIdProvider();

        // Act
        var identifiers = Enumerable.Range(0, 3).Select(_ => provider.GetNextId()).ToArray();

        // Assert
        identifiers.Should().Equal(new EntityId(1), new EntityId(2), new EntityId(3));
    }

    [Fact(DisplayName = "Can continue allocating identifiers after a restored identifier.")]
    [Trait("Category", "Unit")]
    public void CanContinueFromRestoredIdentifier()
    {
        // Arrange
        var provider = new EntityIdProvider();
        provider.SetInitialId(new(42));

        // Act
        var first = provider.GetNextId();
        var second = provider.GetNextId();

        // Assert
        first.Should().Be(new EntityId(43));
        second.Should().Be(new EntityId(44));
    }
}
