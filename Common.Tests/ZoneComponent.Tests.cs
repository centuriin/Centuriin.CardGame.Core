using Centuriin.CardGame.Core.Common.Components;

using FluentAssertions;

using Xunit;

namespace Centuriin.CardGame.Core.Common;

public sealed class ZoneComponentTests
{
    [Fact]
    public void CanCopy()
    {
        // Arrange
        var component = new ZoneComponent(new ZoneId(2));

        // Act
        var copy = component.Copy();

        // Assert
        copy.Should().BeEquivalentTo(component);
    }

    [Fact]
    public void CopyCanNotChangeOriginal()
    {
        // Arrange
        var component = new ZoneComponent(new ZoneId(2));
        var copy = (ZoneComponent)component.Copy();

        // Act
        copy.ChangeSpaceId(new ZoneId(999));

        // Assert
        copy.Should().NotBeEquivalentTo(component);
    }
}