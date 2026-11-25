using Centuriin.CardGame.Core.Engine.Events.Domain;
using Centuriin.CardGame.Core.Engine.Root.State;

using FluentAssertions;

using Moq;

using Xunit;

namespace Centuriin.CardGame.Core.Engine.Tests;

public sealed class ExecutedResultTests
{
    [Fact(DisplayName = "Can be created with an event batch and its version.")]
    [Trait("Category", "Unit")]
    public void CanBeCreated()
    {
        // Arrange
        var version = new GameVersion(3);

        var batch = new Mock<IGameEventsBatch>(MockBehavior.Strict);
        batch.SetupGet(x => x.GameVersion)
            .Returns(version);

        // Act
        var result = new ExecutedResult(batch.Object);

        // Assert
        result.Batch.Should().BeSameAs(batch.Object);
        result.GameVersion.Should().Be(version);
    }

    [Fact(DisplayName = "Cannot be created without an event batch.")]
    [Trait("Category", "Unit")]
    public void CanNotBeCreatedWithoutBatch()
    {
        // Act
        var exception = Record.Exception(() =>
            new ExecutedResult(null!));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }
}
