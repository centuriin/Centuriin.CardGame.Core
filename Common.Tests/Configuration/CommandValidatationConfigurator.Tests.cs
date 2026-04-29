using Centuriin.CardGame.Core.Common.Commands;
using Centuriin.CardGame.Core.Common.Commands.Rules;
using Centuriin.CardGame.Core.Common.Configuration;
using Centuriin.CardGame.Core.Common.Events;
using Centuriin.CardGame.Core.Common.Factories;
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
        var ruleFactoryMock = new Mock<IRuleFactory>(MockBehavior.Strict);
        ruleFactoryMock
            .Setup(x => x.Create<FakeRule>())
            .Returns(fakeRule);

        IReadOnlyDictionary<Type, List<IGameRule>> capturedRules = null!;
        IReadOnlyDictionary<Type, Func<ICommand, IPrimaryEvent>> capturedFactories = null!;

        var validatorMock = new Mock<IConfigurableCommandValidator>(MockBehavior.Strict);
        validatorMock
            .Setup(x => x.Configure(
                It.IsAny<Dictionary<Type, List<IGameRule>>>(),
                It.IsAny<Dictionary<Type, Func<ICommand, IPrimaryEvent>>>()))
            .Callback(
                (IReadOnlyDictionary<Type, List<IGameRule>> rules, IReadOnlyDictionary<Type, Func<ICommand, IPrimaryEvent>> factories) =>
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
        configurator.Setup();

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
            Mock.Of<IRuleFactory>(),
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
        validatorMock.Setup(x => x.Configure(It.IsAny<Dictionary<Type, List<IGameRule>>>(), It.IsAny<Dictionary<Type, Func<ICommand, IPrimaryEvent>>>()));

        var configurator = new CommandValidatationConfigurator(
            Mock.Of<IRuleFactory>(),
            validatorMock.Object);

        var commandRules = configurator.AddValidation<FakeCommand>();
        configurator.Setup();

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

    public sealed record FakeEvent : IPrimaryEvent
    {
        public GameId GameId => default;
    }

    public sealed class FakeRule : IGameRule
    {
        public RuleResult Check(IGameState gameState, ICommand command) => 
            throw new NotSupportedException();
    }
}