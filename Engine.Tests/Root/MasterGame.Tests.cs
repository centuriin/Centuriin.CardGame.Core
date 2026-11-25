using Centuriin.CardGame.Core.Engine.Commands;
using Centuriin.CardGame.Core.Engine.ECS.Entities;
using Centuriin.CardGame.Core.Engine.Events;
using Centuriin.CardGame.Core.Engine.Events.Domain;
using Centuriin.CardGame.Core.Engine.Root;
using Centuriin.CardGame.Core.Engine.Root.State;

using FluentAssertions;

using Moq;

using Xunit;

namespace Centuriin.CardGame.Core.Engine.Tests.Root;

public sealed class MasterGameTests
{
    [Fact(DisplayName = "Can be created with the supplied identity and game state.")]
    [Trait("Category", "Unit")]
    public void CanBeCreated()
    {
        // Arrange
        var gameId = new GameId(Guid.NewGuid());

        var state = Mock.Of<IGameState>(MockBehavior.Strict);

        // Act
        var game = new MasterGame(
            gameId,
            state,
            Mock.Of<IGameEventApplier>(MockBehavior.Strict),
            Mock.Of<IGameEventProducer>(MockBehavior.Strict),
            Mock.Of<ICommandValidator>(MockBehavior.Strict));

        // Assert
        game.Id.Should().Be(gameId);
        game.State.Should().BeSameAs(state);
        game.Version.Should().Be(GameVersion.Default);
    }

    [Fact(DisplayName = "Cannot be created without an event producer.")]
    [Trait("Category", "Unit")]
    public void CanNotBeCreatedWithoutEventProducer()
    {
        // Act
        var exception = Record.Exception(() =>
            new MasterGame(
                new(Guid.NewGuid()),
                Mock.Of<IGameState>(MockBehavior.Strict),
                Mock.Of<IGameEventApplier>(MockBehavior.Strict),
                null!,
                Mock.Of<ICommandValidator>(MockBehavior.Strict)));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = "Can start an authoritative game and return its initial event batch at version one.")]
    [Trait("Category", "Unit")]
    public void CanStartGame()
    {
        // Arrange
        var gameId = new GameId(Guid.NewGuid());

        IGameEvent[] events = [new GameStartedEvent(gameId), new TurnStartedEvent(gameId, new(1))];

        var statusChangeCalls = 0;
        var state = new Mock<IGameState>(MockBehavior.Strict);
        state.SetupGet(x => x.Status)
            .Returns(GameStatus.Created);
        state.Setup(x => x.ChangeStatus(GameStatus.Running))
            .Callback(() => statusChangeCalls++);

        var produceCalls = 0;
        var producer = new Mock<IGameEventProducer>(MockBehavior.Strict);
        producer.Setup(x => x.Produce(It.Is<GameStartedEvent>(e => e.GameId == gameId)))
            .Callback(() => produceCalls++)
            .Returns(events);

        var game = new MasterGame(
            gameId,
            state.Object,
            Mock.Of<IGameEventApplier>(MockBehavior.Strict),
            producer.Object,
            Mock.Of<ICommandValidator>(MockBehavior.Strict));

        // Act
        var result = game.Start();

        // Assert
        var executed = result.Should().BeOfType<ExecutedResult>().Subject;
        executed.Batch.Events.Should().Equal(events);
        executed.GameVersion.Should().Be(new GameVersion(1));
        game.Version.Should().Be(executed.GameVersion);
        statusChangeCalls.Should().Be(1);
        produceCalls.Should().Be(1);
    }

    [Fact(DisplayName = "Can validate an accepted command before producing its events and advancing one version.")]
    [Trait("Category", "Unit")]
    public void CanExecuteCommand()
    {
        // Arrange
        var gameId = new GameId(Guid.NewGuid());

        var commandEvent = new TurnEndedEvent(gameId, new EntityId(1));

        var calls = new List<string>();

        var state = new Mock<IGameState>(MockBehavior.Strict);
        state.SetupGet(x => x.Status)
            .Returns(GameStatus.Running);

        var command = new Mock<ICommand>(MockBehavior.Strict);
        command.SetupGet(x => x.GameId)
            .Returns(gameId);
        command.Setup(x => x.YieldGameEvent())
            .Callback(() => calls.Add("command"))
            .Returns(commandEvent);

        var validator = new Mock<ICommandValidator>(MockBehavior.Strict);
        validator.Setup(x => x.IsValid(command.Object))
            .Callback(() => calls.Add("validate"))
            .Returns(true);

        var producer = new Mock<IGameEventProducer>(MockBehavior.Strict);
        producer.Setup(x => x.Produce(commandEvent))
            .Callback(() => calls.Add("produce"))
            .Returns([commandEvent]);

        var initialBatch = new Mock<IGameEventsBatch>(MockBehavior.Strict);
        initialBatch.SetupGet(x => x.GameVersion)
            .Returns(new GameVersion(1));
        initialBatch.Setup(x => x.GetEnumerator())
            .Returns(() => Enumerable.Empty<IGameEvent>().GetEnumerator());

        var game = new MasterGame(
            gameId,
            state.Object,
            Mock.Of<IGameEventApplier>(MockBehavior.Strict),
            producer.Object,
            validator.Object);
        game.Apply(initialBatch.Object);

        // Act
        var result = game.Execute(command.Object);

        // Assert
        var executed = result.Should().BeOfType<ExecutedResult>().Subject;
        executed.Batch.Events.Should().Equal(commandEvent);
        executed.GameVersion.Should().Be(new GameVersion(2));
        game.Version.Should().Be(executed.GameVersion);
        calls.Should().Equal("validate", "command", "produce");
    }

    [Fact(DisplayName = "Can reject a command without yielding an event or advancing the current version.")]
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

        var initialBatch = new Mock<IGameEventsBatch>(MockBehavior.Strict);
        initialBatch.SetupGet(x => x.GameVersion)
            .Returns(new GameVersion(1));
        initialBatch.Setup(x => x.GetEnumerator())
            .Returns(() => Enumerable.Empty<IGameEvent>().GetEnumerator());

        var game = new MasterGame(
            gameId,
            state.Object,
            Mock.Of<IGameEventApplier>(MockBehavior.Strict),
            Mock.Of<IGameEventProducer>(MockBehavior.Strict),
            validator.Object);
        game.Apply(initialBatch.Object);

        // Act
        var result = game.Execute(command.Object);

        // Assert
        result.Should().BeOfType<RejectedResult>().Which.GameVersion.Should().Be(new GameVersion(1));
        game.Version.Should().Be(new GameVersion(1));
        validationCalls.Should().Be(1);
    }

    [Theory(DisplayName = "Cannot start an authoritative game outside the created state.")]
    [InlineData(GameStatus.Running)]
    [InlineData(GameStatus.Finished)]
    [InlineData(GameStatus.Aborted)]
    [Trait("Category", "Unit")]
    public void CanNotStartOutsideCreatedState(GameStatus status)
    {
        // Arrange
        var state = new Mock<IGameState>(MockBehavior.Strict);
        state.SetupGet(x => x.Status)
            .Returns(status);

        var game = new MasterGame(
            new(Guid.NewGuid()),
            state.Object,
            Mock.Of<IGameEventApplier>(MockBehavior.Strict),
            Mock.Of<IGameEventProducer>(MockBehavior.Strict),
            Mock.Of<ICommandValidator>(MockBehavior.Strict));

        // Act
        var exception = Record.Exception(() =>
            game.Start());

        // Assert
        exception.Should().BeOfType<InvalidOperationException>();
        game.Version.Should().Be(GameVersion.Default);
    }

    [Fact(DisplayName = "Can propagate an event producer failure to the caller.")]
    [Trait("Category", "Unit")]
    public void CanPropagateProducerFailure()
    {
        // Arrange
        var failure = new InvalidOperationException("Event production failed.");

        var state = new Mock<IGameState>(MockBehavior.Strict);
        state.SetupGet(x => x.Status)
            .Returns(GameStatus.Created);
        state.Setup(x => x.ChangeStatus(GameStatus.Running));

        var producer = new Mock<IGameEventProducer>(MockBehavior.Strict);
        producer.Setup(x => x.Produce(It.IsAny<GameStartedEvent>()))
            .Throws(failure);

        var game = new MasterGame(
            new(Guid.NewGuid()),
            state.Object,
            Mock.Of<IGameEventApplier>(MockBehavior.Strict),
            producer.Object,
            Mock.Of<ICommandValidator>(MockBehavior.Strict));

        // Act
        var exception = Record.Exception(() =>
            game.Start());

        // Assert
        exception.Should().BeSameAs(failure);
    }
}
