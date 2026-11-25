using Centuriin.CardGame.Core.Engine.Commands;
using Centuriin.CardGame.Core.Engine.Commands.Rules;
using Centuriin.CardGame.Core.Engine.Loading.Configuration.Commands;

using FluentAssertions;

using Moq;

using Xunit;

namespace Centuriin.CardGame.Core.Engine.Tests.Loading.Configuration.Commands;

public sealed class ConfigurableCommandValidatorTests
{
    [Fact(DisplayName = "Can be created without registered command validations.")]
    [Trait("Category", "Unit")]
    public void CanBeCreated()
    {
        // Arrange
        var builder = new ConfigurableCommandValidator();

        // Act
        var validations = builder.Freeze();

        // Assert
        validations.Should().BeEmpty();
    }

    [Fact(DisplayName = "Can resolve command-specific rules before general rules using their typed factories.")]
    [Trait("Category", "Unit")]
    public void CanCombineSpecificAndGeneralRules()
    {
        // Arrange
        var builder = new ConfigurableCommandValidator();
        builder.AddGeneralRule<IGeneralRule>();
        builder.AddValidation<ICommand>().AddRule<ISpecificRule>();

        var specific = Mock.Of<ISpecificRule>(MockBehavior.Strict);

        var general = Mock.Of<IGeneralRule>(MockBehavior.Strict);

        var calls = new List<string>();
        var factory = new Mock<ICommandRuleFactory>(MockBehavior.Strict);
        factory.Setup(x => x.Create<ISpecificRule>())
            .Callback(() => calls.Add("specific"))
            .Returns(specific);
        factory.Setup(x => x.Create<IGeneralRule>())
            .Callback(() => calls.Add("general"))
            .Returns(general);

        // Act
        var rules = builder.Freeze()[typeof(ICommand)].Select(create => create(factory.Object)).ToArray();

        // Assert
        rules.Should().Equal(specific, general);
        calls.Should().Equal("specific", "general");
    }

    [Fact(DisplayName = "Can register general rules before or after a command validation.")]
    [Trait("Category", "Unit")]
    public void CanAddGeneralRuleAfterCommand()
    {
        // Arrange
        var builder = new ConfigurableCommandValidator();
        builder.AddValidation<ICommand>();
        builder.AddGeneralRule<IGeneralRule>();

        var expected = Mock.Of<IGeneralRule>(MockBehavior.Strict);

        var factory = new Mock<ICommandRuleFactory>(MockBehavior.Strict);
        factory.Setup(x => x.Create<IGeneralRule>())
            .Returns(expected);

        // Act
        var rules = builder.Freeze()[typeof(ICommand)].Select(create => create(factory.Object)).ToArray();

        // Assert
        rules.Should().Equal(expected);
    }

    [Fact(DisplayName = "Cannot register the same command validation twice.")]
    [Trait("Category", "Unit")]
    public void CanNotAddDuplicateCommand()
    {
        // Arrange
        var builder = new ConfigurableCommandValidator();
        builder.AddValidation<ICommand>();

        // Act
        var exception = Record.Exception(() =>
            builder.AddValidation<ICommand>());

        // Assert
        exception.Should().BeOfType<InvalidOperationException>();
    }

    [Fact(DisplayName = "Cannot register the same general rule twice.")]
    [Trait("Category", "Unit")]
    public void CanNotAddDuplicateGeneralRule()
    {
        // Arrange
        var builder = new ConfigurableCommandValidator();
        builder.AddGeneralRule<IGeneralRule>();

        // Act
        var exception = Record.Exception(() =>
            builder.AddGeneralRule<IGeneralRule>());

        // Assert
        exception.Should().BeOfType<InvalidOperationException>();
    }

    [Fact(DisplayName = "Cannot register the same rule twice for one command.")]
    [Trait("Category", "Unit")]
    public void CanNotAddDuplicateSpecificRule()
    {
        // Arrange
        var validation = new ConfigurableCommandValidator().AddValidation<ICommand>();
        validation.AddRule<ISpecificRule>();

        // Act
        var exception = Record.Exception(() =>
            validation.AddRule<ISpecificRule>());

        // Assert
        exception.Should().BeOfType<InvalidOperationException>();
    }

    [Fact(DisplayName = "Cannot register command validations after freezing.")]
    [Trait("Category", "Unit")]
    public void CanNotAddCommandAfterFreeze()
    {
        // Arrange
        var builder = new ConfigurableCommandValidator();
        _ = builder.Freeze();

        // Act
        var exception = Record.Exception(() =>
            builder.AddValidation<ICommand>());

        // Assert
        exception.Should().BeOfType<InvalidOperationException>();
    }

    [Fact(DisplayName = "Cannot register general rules after freezing.")]
    [Trait("Category", "Unit")]
    public void CanNotAddGeneralRuleAfterFreeze()
    {
        // Arrange
        var builder = new ConfigurableCommandValidator();
        _ = builder.Freeze();

        // Act
        var exception = Record.Exception(() =>
            builder.AddGeneralRule<IGeneralRule>());

        // Assert
        exception.Should().BeOfType<InvalidOperationException>();
    }

    [Fact(DisplayName = "Cannot add rules through a retained command builder after freezing.")]
    [Trait("Category", "Unit")]
    public void CanNotAddSpecificRuleAfterFreeze()
    {
        // Arrange
        var builder = new ConfigurableCommandValidator();

        var validation = builder.AddValidation<ICommand>();
        _ = builder.Freeze();

        // Act
        var exception = Record.Exception(() =>
            validation.AddRule<ISpecificRule>());

        // Assert
        exception.Should().BeOfType<InvalidOperationException>();
    }

    public interface ISpecificRule : ICommandRule<ICommand>;

    public interface IGeneralRule : ICommandRule<ICommand>;
}
