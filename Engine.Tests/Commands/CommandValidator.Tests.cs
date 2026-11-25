using System.Collections.Immutable;

using Centuriin.CardGame.Core.Engine.Commands;
using Centuriin.CardGame.Core.Engine.Commands.Rules;
using Centuriin.CardGame.Core.Engine.Observability.Logging;
using Centuriin.CardGame.Core.Engine.Root.State;

using FluentAssertions;

using Moq;

using Xunit;

namespace Centuriin.CardGame.Core.Engine.Tests.Commands;

public sealed class CommandValidatorTests
{
    [Fact(DisplayName = "Can be created.")]
    [Trait("Category", "Unit")]
    public void CanBeCreated()
    {
        // Act
        var exception = Record.Exception(() =>
            new CommandValidator(
                Mock.Of<ICoreLogger<CommandValidator>>(MockBehavior.Strict),
                Mock.Of<IReadOnlyGameState>(MockBehavior.Strict)));

        // Assert
        exception.Should().BeNull();
    }

    [Fact(DisplayName = "Cannot be created without a logger.")]
    [Trait("Category", "Unit")]
    public void CanNotBeCreatedWithoutLogger()
    {
        // Act
        var exception = Record.Exception(() =>
            new CommandValidator(
                null!,
                Mock.Of<IReadOnlyGameState>(MockBehavior.Strict)));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = "Cannot be created without game state.")]
    [Trait("Category", "Unit")]
    public void CanNotBeCreatedWithoutGameState()
    {
        // Act
        var exception = Record.Exception(() =>
            new CommandValidator(
                Mock.Of<ICoreLogger<CommandValidator>>(MockBehavior.Strict),
                null!));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = "Can validate registered rules in their configured order using the supplied state.")]
    [Trait("Category", "Unit")]
    public void CanValidateRulesInOrder()
    {
        // Arrange
        var state = Mock.Of<IReadOnlyGameState>(MockBehavior.Strict);

        var command = Mock.Of<ICommand>(MockBehavior.Strict);

        var calls = new List<string>();
        var first = new Mock<ICommandRule>(MockBehavior.Strict);
        first.Setup(x => x.Check(command, state))
            .Callback(() => calls.Add("first"))
            .Returns(true);

        var second = new Mock<ICommandRule>(MockBehavior.Strict);
        second.Setup(x => x.Check(command, state))
            .Callback(() => calls.Add("second"))
            .Returns(true);

        var logger = new Mock<ICoreLogger<CommandValidator>>(MockBehavior.Strict);
        logger.Setup(x => x.IsEnabled(LogLevel.Debug))
            .Returns(false);

        var validator = new CommandValidator(
            logger.Object,
            state);
        validator.AddValidation(command.GetType(), [first.Object, second.Object]);

        // Act
        var result = validator.IsValid(command);

        // Assert
        result.Should().BeTrue();
        calls.Should().Equal("first", "second");
    }

    [Fact(DisplayName = "Can reject a command at the first failing rule without invoking later rules.")]
    [Trait("Category", "Unit")]
    public void CanStopAtFirstFailingRule()
    {
        // Arrange
        var state = Mock.Of<IReadOnlyGameState>(MockBehavior.Strict);

        var command = Mock.Of<ICommand>(MockBehavior.Strict);

        var ruleCalls = 0;
        var first = new Mock<ICommandRule>(MockBehavior.Strict);
        first.Setup(x => x.Check(command, state))
            .Callback(() => ruleCalls++)
            .Returns(false);

        var warningCalls = 0;
        var logger = new Mock<ICoreLogger<CommandValidator>>(MockBehavior.Strict);
        logger.Setup(x => x.IsEnabled(LogLevel.Debug))
            .Returns(false);
        logger.Setup(x => x.LogWarning(It.IsAny<string>(), It.IsAny<object[]>()))
            .Callback(() => warningCalls++);

        var validator = new CommandValidator(
            logger.Object,
            state);
        validator.AddValidation(
            command.GetType(),
            [first.Object, Mock.Of<ICommandRule>(MockBehavior.Strict)]);

        // Act
        var result = validator.IsValid(command);

        // Assert
        result.Should().BeFalse();
        ruleCalls.Should().Be(1);
        warningCalls.Should().Be(1);
    }

    [Fact(DisplayName = "Can accept a registered command with no validation rules.")]
    [Trait("Category", "Unit")]
    public void CanValidateCommandWithoutRules()
    {
        // Arrange
        var command = Mock.Of<ICommand>(MockBehavior.Strict);

        var logger = new Mock<ICoreLogger<CommandValidator>>(MockBehavior.Strict);
        logger.Setup(x => x.IsEnabled(LogLevel.Debug))
            .Returns(false);

        var validator = new CommandValidator(
            logger.Object,
            Mock.Of<IReadOnlyGameState>(MockBehavior.Strict));
        validator.AddValidation(command.GetType(), []);

        // Act
        var result = validator.IsValid(command);

        // Assert
        result.Should().BeTrue();
    }

    [Fact(DisplayName = "Cannot validate an unregistered command type.")]
    [Trait("Category", "Unit")]
    public void CanNotValidateUnregisteredCommand()
    {
        // Arrange
        var command = Mock.Of<ICommand>(MockBehavior.Strict);

        var validator = new CommandValidator(
            Mock.Of<ICoreLogger<CommandValidator>>(MockBehavior.Strict),
            Mock.Of<IReadOnlyGameState>(MockBehavior.Strict));

        // Act
        var exception = Record.Exception(() =>
            validator.IsValid(command));

        // Assert
        exception.Should().BeOfType<InvalidOperationException>();
    }

    [Fact(DisplayName = "Cannot validate a null command.")]
    [Trait("Category", "Unit")]
    public void CanNotValidateNullCommand()
    {
        // Arrange
        var validator = new CommandValidator(
            Mock.Of<ICoreLogger<CommandValidator>>(MockBehavior.Strict),
            Mock.Of<IReadOnlyGameState>(MockBehavior.Strict));

        // Act
        var exception = Record.Exception(() =>
            validator.IsValid(null!));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = "Cannot register validation without a command type.")]
    [Trait("Category", "Unit")]
    public void CanNotRegisterWithoutCommandType()
    {
        // Arrange
        var validator = new CommandValidator(
            Mock.Of<ICoreLogger<CommandValidator>>(MockBehavior.Strict),
            Mock.Of<IReadOnlyGameState>(MockBehavior.Strict));

        // Act
        var exception = Record.Exception(() =>
            validator.AddValidation(null!, []));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = "Cannot register validation without a rule collection.")]
    [Trait("Category", "Unit")]
    public void CanNotRegisterWithoutRules()
    {
        // Arrange
        var validator = new CommandValidator(
            Mock.Of<ICoreLogger<CommandValidator>>(MockBehavior.Strict),
            Mock.Of<IReadOnlyGameState>(MockBehavior.Strict));

        // Act
        var exception = Record.Exception(() =>
            validator.AddValidation(typeof(ICommand), null!));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = "Cannot register a command type twice or replace its original validation.")]
    [Trait("Category", "Unit")]
    public void CanNotRegisterDuplicateValidation()
    {
        // Arrange
        var command = Mock.Of<ICommand>(MockBehavior.Strict);

        var logger = new Mock<ICoreLogger<CommandValidator>>(MockBehavior.Strict);
        logger.Setup(x => x.IsEnabled(LogLevel.Debug))
            .Returns(false);

        var validator = new CommandValidator(
            logger.Object,
            Mock.Of<IReadOnlyGameState>(MockBehavior.Strict));
        validator.AddValidation(command.GetType(), ImmutableList<ICommandRule>.Empty);

        // Act
        var exception = Record.Exception(() =>
            validator.AddValidation(
                command.GetType(),
                [Mock.Of<ICommandRule>(MockBehavior.Strict)]));
        var isValid = validator.IsValid(command);

        // Assert
        exception.Should().BeOfType<InvalidOperationException>();
        isValid.Should().BeTrue();
    }

    [Fact(DisplayName = "Can propagate a rule failure without evaluating later rules.")]
    [Trait("Category", "Unit")]
    public void CanPropagateRuleFailure()
    {
        // Arrange
        var state = Mock.Of<IReadOnlyGameState>(MockBehavior.Strict);

        var command = Mock.Of<ICommand>(MockBehavior.Strict);

        var failure = new InvalidOperationException("Rule failed.");

        var first = new Mock<ICommandRule>(MockBehavior.Strict);
        first.Setup(x => x.Check(command, state))
            .Throws(failure);

        var logger = new Mock<ICoreLogger<CommandValidator>>(MockBehavior.Strict);
        logger.Setup(x => x.IsEnabled(LogLevel.Debug))
            .Returns(false);

        var validator = new CommandValidator(
            logger.Object,
            state);
        validator.AddValidation(
            command.GetType(),
            [first.Object, Mock.Of<ICommandRule>(MockBehavior.Strict)]);

        // Act
        var exception = Record.Exception(() =>
            validator.IsValid(command));

        // Assert
        exception.Should().BeSameAs(failure);
    }
}
