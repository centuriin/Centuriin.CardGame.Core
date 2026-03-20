using Centuriin.CardGame.Core.Common.Components;

using FluentAssertions;

using Xunit;

namespace Centuriin.CardGame.Core.Common;

public sealed class SpaceComponentTests
{
    [Fact]
    public void CanCopy()
    {
        // Arrange
        var component = new SpaceComponent(new SpaceId(2));

        // Act
        var copy = component.Copy();

        // Assert
        copy.Should().BeEquivalentTo(component);
    }

    [Fact]
    public void CopyCanNotChangeOriginal()
    {
        // Arrange
        var component = new SpaceComponent(new SpaceId(2));
        var copy = (SpaceComponent)component.Copy();

        // Act
        copy.ChangeSpaceId(new SpaceId(999));

        // Assert
        copy.Should().NotBeEquivalentTo(component);
    }
}