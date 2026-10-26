using Centuriin.CardGame.Core.Common.Commands;
using Centuriin.CardGame.Core.Common.Commands.Rules;
using Centuriin.CardGame.Core.Common.Configuration;
using Centuriin.CardGame.Core.Common.Events.Domain;
using Centuriin.CardGame.Core.Common.World;

using FluentAssertions;

using Moq;

using Xunit;

namespace Centuriin.CardGame.Core.Common.Tests.Configuration;

public sealed class CommandValidatationConfiguratorTests
{
    [Fact]
    public void SetupShouldConfigureValidatorWithCorrectRulesAndFactories()
    {
        // Arrange
        var fakeRule = new FakeRule();
        var ruleFactoryMock = new Mock<ICommandRuleFactory>(MockBehavior.Strict);
        ruleFactoryMock
            .Setup(x => x.Create<FakeRule>())
            .Returns(fakeRule);

        IReadOnlyDictionary<Type, List<ICommandRule>> capturedRules = null!;
        IReadOnlyDictionary<Type, Func<ICommand, IGameEvent>> capturedFactories = null!;

        var validatorMock = new Mock<IConfigurableCommandValidator>(MockBehavior.Strict);
        validatorMock
            .Setup(x => x.Configure(
                It.IsAny<Dictionary<Type, List<ICommandRule>>>(),
                It.IsAny<Dictionary<Type, Func<ICommand, IGameEvent>>>()))
            .Callback(
                (IReadOnlyDictionary<Type, List<ICommandRule>> rules, IReadOnlyDictionary<Type, Func<ICommand, IGameEvent>> factories) =>
                {
                    capturedRules = rules;
                    capturedFactories = factories;
                });

        var configurator = new CommandValidatationConfigurator(
            ruleFactoryMock.Object,
            validatorMock.Object);

        configurator.AddValidation<FakeCommand>()
            .AddRule<FakeRule>()
            .WithFactory(cmd => new FakeEvent());

        // Act
        configurator.Configure();

        // Assert
        capturedRules.Should().ContainKey(typeof(FakeCommand));
        capturedRules[typeof(FakeCommand)].Should().Contain(fakeRule);

        capturedFactories.Should().ContainKey(typeof(FakeCommand));
        capturedFactories[typeof(FakeCommand)].Invoke(new FakeCommand()).Should().BeOfType<FakeEvent>();
    }

    [Fact]
    public void WithFactoryShouldThrowWhenDuplicateFactoryRegisteredForSameCommand()
    {
        // Arrange
        var configurator = new CommandValidatationConfigurator(
            Mock.Of<ICommandRuleFactory>(),
            Mock.Of<IConfigurableCommandValidator>());

        var commandRules = configurator.AddValidation<FakeCommand>();
        commandRules.WithFactory(cmd => new FakeEvent());

        // Act
        var exception = Record.Exception(() =>
            commandRules.WithFactory(cmd => new FakeEvent()));

        // Assert
        exception.Should().NotBeNull();
        exception.Should().BeOfType<InvalidOperationException>();
    }

    [Fact]
    public void AddRuleShouldThrowIfConfiguratorIsAlreadyInitialized()
    {
        // Arrange
        var validatorMock = new Mock<IConfigurableCommandValidator>(MockBehavior.Strict);
        validatorMock.Setup(x => x.Configure(It.IsAny<Dictionary<Type, List<ICommandRule>>>(), It.IsAny<Dictionary<Type, Func<ICommand, IGameEvent>>>()));

        var configurator = new CommandValidatationConfigurator(
            Mock.Of<ICommandRuleFactory>(),
            validatorMock.Object);

        var commandRules = configurator.AddValidation<FakeCommand>();
        configurator.Configure();

        // Act
        var exception = Record.Exception(() =>
            commandRules.AddRule<FakeRule>());

        // Assert
        exception.Should().NotBeNull();
        exception.Should().BeOfType<InvalidOperationException>();
    }

    public sealed record FakeCommand : ICommand
    {
        public GameId GameId => default;

        public PlayerId Actor => default;
    }

    public sealed record FakeEvent : IGameEvent
    {
        public GameId GameId => default;
    }

    public sealed class FakeRule : ICommandRule
    {
        public RuleResult Check(IGameState gameState, ICommand command) => 
            throw new NotSupportedException();
    }
}