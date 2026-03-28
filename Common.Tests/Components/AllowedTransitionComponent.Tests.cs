using Centuriin.CardGame.Core.Common.Entities.Zones;

using FluentAssertions;

using Xunit;

namespace Centuriin.CardGame.Core.Common.Components;

public sealed class AllowedTransitionComponentTests
{
    [Fact]
    public void CanCopy()
    {
        // Arrange
        var component = new AllowedTransitionComponent<ZoneId>(
            new HashSet<ZoneId> { new(1) });

        // Act
        var copy = component.Copy();

        // Assert
        copy.Should().BeEquivalentTo(component);
    }

    [Fact]
    public void CopyCanNotChangeOriginal()
    {
        // Arrange
        var set = new HashSet<ZoneId> { new(1) };
        var component = new AllowedTransitionComponent<ZoneId>(set);

        var copy = (AllowedTransitionComponent<ZoneId>)component.Copy();

        // Act
        copy.AllowedFrom.Add(new ZoneId(999));

        // Assert
        copy.Should().NotBeEquivalentTo(component);
    }
}