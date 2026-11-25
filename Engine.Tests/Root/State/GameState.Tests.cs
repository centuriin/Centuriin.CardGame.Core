using System.ComponentModel;

using Centuriin.CardGame.Core.Engine.Root.State;

using FluentAssertions;

using Moq;

using Xunit;

namespace Centuriin.CardGame.Core.Engine.Tests.Root.State;

public sealed class GameStateTests
{
    [Fact(DisplayName = "Can be created with an empty entity collection and shared read-only views.")]
    [Trait("Category", "Unit")]
    public void CanBeCreated()
    {
        // Arrange
        var automat = Mock.Of<ITurnAutomat>(MockBehavior.Strict);

        // Act
        var state = new GameState(automat);

        // Assert
        state.Status.Should().Be(GameStatus.Created);
        state.Entities.Should().BeEmpty();
        state.TurnAutomat.Should().BeSameAs(automat);
        ((IReadOnlyGameState)state).TurnAutomat.Should().BeSameAs(automat);
        ((IReadOnlyGameState)state).Entities.Should().BeSameAs(state.Entities);
    }

    [Fact(DisplayName = "Cannot be created without a turn controller.")]
    [Trait("Category", "Unit")]
    public void CanNotBeCreatedWithoutTurnAutomat()
    {
        // Act
        var exception = Record.Exception(() =>
            new GameState(null!));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Theory(DisplayName = "Can assign a defined game status.")]
    [InlineData(GameStatus.None)]
    [InlineData(GameStatus.Created)]
    [InlineData(GameStatus.Running)]
    [InlineData(GameStatus.Finished)]
    [InlineData(GameStatus.Aborted)]
    [Trait("Category", "Unit")]
    public void CanChangeStatus(GameStatus status)
    {
        // Arrange
        var state = new GameState(Mock.Of<ITurnAutomat>(MockBehavior.Strict));

        // Act
        state.ChangeStatus(status);

        // Assert
        state.Status.Should().Be(status);
    }

    [Fact(DisplayName = "Cannot assign an undefined status or change the previous status on failure.")]
    [Trait("Category", "Unit")]
    public void CanNotChangeToUndefinedStatus()
    {
        // Arrange
        var state = new GameState(Mock.Of<ITurnAutomat>(MockBehavior.Strict));

        // Act
        var exception = Record.Exception(() =>
            state.ChangeStatus((GameStatus)99));

        // Assert
        exception.Should().BeOfType<InvalidEnumArgumentException>();
        state.Status.Should().Be(GameStatus.Created);
    }
}
