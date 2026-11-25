using Centuriin.CardGame.Core.Engine.Commands;
using Centuriin.CardGame.Core.Engine.Events;
using Centuriin.CardGame.Core.Engine.Events.Domain;
using Centuriin.CardGame.Core.Engine.Root;
using Centuriin.CardGame.Core.Engine.Root.State;

using FluentAssertions;

using Moq;

using Xunit;

namespace Centuriin.CardGame.Core.Engine.Tests.Root;

public sealed class ReplicaGameTests
{
    [Fact(DisplayName = "Can start a replica without applying events or advancing its version.")]
    [Trait("Category", "Unit")]
    public void CanStartReplica()
    {
        // Arrange
        var statusChangeCalls = 0;
        var state = new Mock<IGameState>(MockBehavior.Strict);
        state.SetupGet(x => x.Status)
            .Returns(GameStatus.Created);
        state.Setup(x => x.ChangeStatus(GameStatus.Running))
            .Callback(() => statusChangeCalls++);

        var game = new ReplicaGame(
            new(Guid.NewGuid()),
            state.Object,
            Mock.Of<IGameEventApplier>(MockBehavior.Strict),
            Mock.Of<ICommandValidator>(MockBehavior.Strict));

        // Act
        var result = game.Start();

        // Assert
        result.Should().BeOfType<PreparedResult>().Which.GameVersion.Should().Be(GameVersion.Default);
        statusChangeCalls.Should().Be(1);
        game.Version.Should().Be(GameVersion.Default);
    }

    [Fact(DisplayName = "Can prepare a valid command without yielding or applying its domain event.")]
    [Trait("Category", "Unit")]
    public void CanPrepareCommand()
    {
        // Arrange
        var gameId = new GameId(Guid.NewGuid());

        var state = new Mock<IGameState>(MockBehavior.Strict);
        state.SetupGet(x => x.Status)
            .Returns(GameStatus.Running);

        var command = new Mock<ICommand>(MockBehavior.Strict);
        command.SetupGet(x => x.GameId)
            .Returns(gameId);

        var validationCalls = 0;
        var validator = new Mock<ICommandValidator>(MockBehavior.Strict);
        validator.Setup(x => x.IsValid(command.Object))
            .Callback(() => validationCalls++)
            .Returns(true);

        var game = new ReplicaGame(
            gameId,
            state.Object,
            Mock.Of<IGameEventApplier>(MockBehavior.Strict),
            validator.Object);

        // Act
        var result = game.Execute(command.Object);

        // Assert
        result.Should().BeOfType<PreparedResult>().Which.GameVersion.Should().Be(GameVersion.Default);
        game.Version.Should().Be(GameVersion.Default);
        validationCalls.Should().Be(1);
    }

    [Fact(DisplayName = "Can reject a command on a replica without applying it.")]
    [Trait("Category", "Unit")]
    public void CanRejectCommand()
    {
        // Arrange
        var gameId = new GameId(Guid.NewGuid());

        var state = new Mock<IGameState>(MockBehavior.Strict);
        state.SetupGet(x => x.Status)
            .Returns(GameStatus.Running);

        var command = new Mock<ICommand>(MockBehavior.Strict);
        command.SetupGet(x => x.GameId)
            .Returns(gameId);

        var validationCalls = 0;
        var validator = new Mock<ICommandValidator>(MockBehavior.Strict);
        validator.Setup(x => x.IsValid(command.Object))
            .Callback(() => validationCalls++)
            .Returns(false);

        var game = new ReplicaGame(
            gameId,
            state.Object,
            Mock.Of<IGameEventApplier>(MockBehavior.Strict),
            validator.Object);

        // Act
        var result = game.Execute(command.Object);

        // Assert
        result.Should().BeOfType<RejectedResult>().Which.GameVersion.Should().Be(GameVersion.Default);
        validationCalls.Should().Be(1);
    }

    [Fact(DisplayName = "Can apply consecutive event batches in order and update the game version.")]
    [Trait("Category", "Unit")]
    public void CanApplyConsecutiveBatches()
    {
        // Arrange
        var gameId = new GameId(Guid.NewGuid());

        IGameEvent[] firstEvents = [new GameStartedEvent(gameId), new TurnStartedEvent(gameId, new(1))];

        IGameEvent[] secondEvents = [new TurnEndedEvent(gameId, new(1))];

        var state = new Mock<IGameState>(MockBehavior.Strict);
        state.SetupGet(x => x.Status)
            .Returns(GameStatus.Running);

        var firstBatch = new Mock<IGameEventsBatch>(MockBehavior.Strict);
        firstBatch.SetupGet(x => x.GameVersion)
            .Returns(new GameVersion(1));
        firstBatch.Setup(x => x.GetEnumerator())
            .Returns(() => ((IEnumerable<IGameEvent>)firstEvents).GetEnumerator());

        var secondBatch = new Mock<IGameEventsBatch>(MockBehavior.Strict);
        secondBatch.SetupGet(x => x.GameVersion)
            .Returns(new GameVersion(2));
        secondBatch.Setup(x => x.GetEnumerator())
            .Returns(() => ((IEnumerable<IGameEvent>)secondEvents).GetEnumerator());

        var applied = new List<IGameEvent>();
        var applier = new Mock<IGameEventApplier>(MockBehavior.Strict);
        applier.Setup(x => x.Apply(It.IsAny<IGameEvent>()))
            .Callback<IGameEvent>(applied.Add);

        var game = new ReplicaGame(
            gameId,
            state.Object,
            applier.Object,
            Mock.Of<ICommandValidator>(MockBehavior.Strict));

        // Act
        game.Apply(firstBatch.Object);
        game.Apply(secondBatch.Object);

        // Assert
        applied.Should().Equal(firstEvents.Concat(secondEvents));
        game.Version.Should().Be(new GameVersion(2));
    }

    [Theory(DisplayName = "Cannot apply a batch whose version is not the next game version.")]
    [InlineData(0)]
    [InlineData(2)]
    [Trait("Category", "Unit")]
    public void CanNotApplyUnexpectedVersion(int version)
    {
        // Arrange
        var batch = new Mock<IGameEventsBatch>(MockBehavior.Strict);
        batch.SetupGet(x => x.GameVersion)
            .Returns(new GameVersion(version));

        var game = new ReplicaGame(
            new(Guid.NewGuid()),
            Mock.Of<IGameState>(MockBehavior.Strict),
            Mock.Of<IGameEventApplier>(MockBehavior.Strict),
            Mock.Of<ICommandValidator>(MockBehavior.Strict));

        // Act
        var exception = Record.Exception(() =>
            game.Apply(batch.Object));

        // Assert
        exception.Should().BeOfType<InvalidOperationException>();
        game.Version.Should().Be(GameVersion.Default);
    }

    [Fact(DisplayName = "Cannot apply the same event batch twice.")]
    [Trait("Category", "Unit")]
    public void CanNotApplyDuplicateBatch()
    {
        // Arrange
        IGameEvent[] events = [new GameStartedEvent(new(Guid.NewGuid()))];

        var state = new Mock<IGameState>(MockBehavior.Strict);
        state.SetupGet(x => x.Status)
            .Returns(GameStatus.Running);

        var batch = new Mock<IGameEventsBatch>(MockBehavior.Strict);
        batch.SetupGet(x => x.GameVersion)
            .Returns(new GameVersion(1));
        batch.Setup(x => x.GetEnumerator())
            .Returns(() => ((IEnumerable<IGameEvent>)events).GetEnumerator());

        var appliedCalls = 0;
        var applier = new Mock<IGameEventApplier>(MockBehavior.Strict);
        applier.Setup(x => x.Apply(events[0]))
            .Callback(() => appliedCalls++);

        var game = new ReplicaGame(
            new(Guid.NewGuid()),
            state.Object,
            applier.Object,
            Mock.Of<ICommandValidator>(MockBehavior.Strict));
        game.Apply(batch.Object);

        // Act
        var exception = Record.Exception(() =>
            game.Apply(batch.Object));

        // Assert
        exception.Should().BeOfType<InvalidOperationException>();
        appliedCalls.Should().Be(1);
        game.Version.Should().Be(new GameVersion(1));
    }

    [Fact(DisplayName = "Cannot apply a null event batch.")]
    [Trait("Category", "Unit")]
    public void CanNotApplyNullBatch()
    {
        // Arrange
        var game = new ReplicaGame(
            new(Guid.NewGuid()),
            Mock.Of<IGameState>(MockBehavior.Strict),
            Mock.Of<IGameEventApplier>(MockBehavior.Strict),
            Mock.Of<ICommandValidator>(MockBehavior.Strict));

        // Act
        var exception = Record.Exception(() =>
            game.Apply(null!));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = "Cannot apply events before a replica has started.")]
    [Trait("Category", "Unit")]
    public void CanNotApplyBeforeStart()
    {
        // Arrange
        var state = new Mock<IGameState>(MockBehavior.Strict);
        state.SetupGet(x => x.Status)
            .Returns(GameStatus.Created);

        var batch = new Mock<IGameEventsBatch>(MockBehavior.Strict);
        batch.SetupGet(x => x.GameVersion)
            .Returns(new GameVersion(1));

        var game = new ReplicaGame(
            new(Guid.NewGuid()),
            state.Object,
            Mock.Of<IGameEventApplier>(MockBehavior.Strict),
            Mock.Of<ICommandValidator>(MockBehavior.Strict));

        // Act
        var exception = Record.Exception(() =>
            game.Apply(batch.Object));

        // Assert
        exception.Should().BeOfType<InvalidOperationException>();
        game.Version.Should().Be(GameVersion.Default);
    }

    [Fact(DisplayName = "Cannot execute a command intended for another game.")]
    [Trait("Category", "Unit")]
    public void CanNotExecuteForeignCommand()
    {
        // Arrange
        var gameId = new GameId(Guid.NewGuid());

        var foreignId = new GameId(Guid.NewGuid());

        var command = new Mock<ICommand>(MockBehavior.Strict);
        command.SetupGet(x => x.GameId)
            .Returns(foreignId);

        var game = new ReplicaGame(
            gameId,
            Mock.Of<IGameState>(MockBehavior.Strict),
            Mock.Of<IGameEventApplier>(MockBehavior.Strict),
            Mock.Of<ICommandValidator>(MockBehavior.Strict));

        // Act
        var exception = Record.Exception(() =>
            game.Execute(command.Object));

        // Assert
        exception.Should().BeOfType<InvalidOperationException>();
        game.Version.Should().Be(GameVersion.Default);
    }

    [Fact(DisplayName = "Cannot execute a null command.")]
    [Trait("Category", "Unit")]
    public void CanNotExecuteNullCommand()
    {
        // Arrange
        var game = new ReplicaGame(
            new(Guid.NewGuid()),
            Mock.Of<IGameState>(MockBehavior.Strict),
            Mock.Of<IGameEventApplier>(MockBehavior.Strict),
            Mock.Of<ICommandValidator>(MockBehavior.Strict));

        // Act
        var exception = Record.Exception(() =>
            game.Execute(null!));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Theory(DisplayName = "Cannot execute commands outside the running game state.")]
    [InlineData(GameStatus.Created)]
    [InlineData(GameStatus.Finished)]
    [InlineData(GameStatus.Aborted)]
    [Trait("Category", "Unit")]
    public void CanNotExecuteOutsideRunningState(GameStatus status)
    {
        // Arrange
        var gameId = new GameId(Guid.NewGuid());

        var state = new Mock<IGameState>(MockBehavior.Strict);
        state.SetupGet(x => x.Status)
            .Returns(status);

        var command = new Mock<ICommand>(MockBehavior.Strict);
        command.SetupGet(x => x.GameId)
            .Returns(gameId);

        var game = new ReplicaGame(
            gameId,
            state.Object,
            Mock.Of<IGameEventApplier>(MockBehavior.Strict),
            Mock.Of<ICommandValidator>(MockBehavior.Strict));

        // Act
        var exception = Record.Exception(() =>
            game.Execute(command.Object));

        // Assert
        exception.Should().BeOfType<InvalidOperationException>();
    }

    [Fact(DisplayName = "Can stop batch application on failure without advancing the batch version.")]
    [Trait("Category", "Unit")]
    public void CanPropagateEventApplicationFailure()
    {
        // Arrange
        var gameId = new GameId(Guid.NewGuid());

        var first = new GameStartedEvent(gameId);

        var second = new TurnStartedEvent(gameId, new(1));

        IGameEvent[] events = [first, second, new TurnEndedEvent(gameId, new(1))];

        var failure = new InvalidOperationException("Event application failed.");

        var state = new Mock<IGameState>(MockBehavior.Strict);
        state.SetupGet(x => x.Status)
            .Returns(GameStatus.Running);

        var batch = new Mock<IGameEventsBatch>(MockBehavior.Strict);
        batch.SetupGet(x => x.GameVersion)
            .Returns(new GameVersion(1));
        batch.Setup(x => x.GetEnumerator())
            .Returns(() => ((IEnumerable<IGameEvent>)events).GetEnumerator());

        var applied = new List<IGameEvent>();
        var applier = new Mock<IGameEventApplier>(MockBehavior.Strict);
        applier.Setup(x => x.Apply(first))
            .Callback(() => applied.Add(first));
        applier.Setup(x => x.Apply(second))
            .Callback(() => applied.Add(second))
            .Throws(failure);

        var game = new ReplicaGame(
            gameId,
            state.Object,
            applier.Object,
            Mock.Of<ICommandValidator>(MockBehavior.Strict));

        // Act
        var exception = Record.Exception(() =>
            game.Apply(batch.Object));

        // Assert
        exception.Should().BeSameAs(failure);
        applied.Should().Equal(first, second);
        game.Version.Should().Be(GameVersion.Default);
    }
}
