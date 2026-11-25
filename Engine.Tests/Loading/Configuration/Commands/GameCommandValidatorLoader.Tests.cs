using System.Collections.Immutable;

using Centuriin.CardGame.Core.Engine.Commands;
using Centuriin.CardGame.Core.Engine.Commands.Rules;
using Centuriin.CardGame.Core.Engine.Context;
using Centuriin.CardGame.Core.Engine.Loading.Configuration;
using Centuriin.CardGame.Core.Engine.Loading.Configuration.Commands;

using FluentAssertions;

using Moq;

using Xunit;

namespace Centuriin.CardGame.Core.Engine.Tests.Loading.Configuration.Commands;

public sealed class GameCommandValidatorLoaderTests
{
    [Fact(DisplayName = "Can be created.")]
    [Trait("Category", "Unit")]
    public void CanBeCreated()
    {
        // Act
        var exception = Record.Exception(() =>
            new GameCommandValidatorLoader(
                Mock.Of<IGameContextAccessor<IGameProfileContext>>(MockBehavior.Strict),
                Mock.Of<ICommandRuleFactory>(MockBehavior.Strict),
                Mock.Of<ICommandValidator>(MockBehavior.Strict)));

        // Assert
        exception.Should().BeNull();
    }

    [Fact(DisplayName = "Cannot be created without a profile context accessor.")]
    [Trait("Category", "Unit")]
    public void CanNotBeCreatedWithoutContextAccessor()
    {
        // Act
        var exception = Record.Exception(() =>
            new GameCommandValidatorLoader(
                null!,
                Mock.Of<ICommandRuleFactory>(MockBehavior.Strict),
                Mock.Of<ICommandValidator>(MockBehavior.Strict)));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = "Cannot be created without a command rule factory.")]
    [Trait("Category", "Unit")]
    public void CanNotBeCreatedWithoutRuleFactory()
    {
        // Act
        var exception = Record.Exception(() =>
            new GameCommandValidatorLoader(
                Mock.Of<IGameContextAccessor<IGameProfileContext>>(MockBehavior.Strict),
                null!,
                Mock.Of<ICommandValidator>(MockBehavior.Strict)));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = "Cannot be created without a command validator.")]
    [Trait("Category", "Unit")]
    public void CanNotBeCreatedWithoutValidator()
    {
        // Act
        var exception = Record.Exception(() =>
            new GameCommandValidatorLoader(
                Mock.Of<IGameContextAccessor<IGameProfileContext>>(MockBehavior.Strict),
                Mock.Of<ICommandRuleFactory>(MockBehavior.Strict),
                null!));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = "Can resolve configured rules and register them for the selected command type.")]
    [Trait("Category", "Unit")]
    public async Task CanLoadCommandValidationsAsync()
    {
        // Arrange
        var gameId = new GameId(Guid.NewGuid());
        var token = TestContext.Current.CancellationToken;

        var expectedRule = Mock.Of<ICommandRule<ICommand>>(MockBehavior.Strict);

        var profile = new Mock<IGameProfile>(MockBehavior.Strict);
        profile.Setup(x => x.Configure(It.IsAny<IConfigurableCommandValidator>()))
            .Callback<IConfigurableCommandValidator>(builder =>
            {
                builder.AddGeneralRule<ICommandRule<ICommand>>();
                builder.AddValidation<ICommand>();
            });

        var context = new Mock<IGameProfileContext>(MockBehavior.Strict);
        context.SetupGet(x => x.Profile)
            .Returns(profile.Object);

        var accessor = new Mock<IGameContextAccessor<IGameProfileContext>>(MockBehavior.Strict);
        accessor.Setup(x => x.GetContextAsync(gameId, token))
            .ReturnsAsync(context.Object);

        var factoryCalls = 0;
        var factory = new Mock<ICommandRuleFactory>(MockBehavior.Strict);
        factory.Setup(x => x.Create<ICommandRule<ICommand>>())
            .Callback(() => factoryCalls++)
            .Returns(expectedRule);

        ImmutableList<ICommandRule>? capturedRules = null;
        var registrationCalls = 0;
        var validator = new Mock<ICommandValidator>(MockBehavior.Strict);
        validator.Setup(x => x.AddValidation(typeof(ICommand), It.IsAny<ImmutableList<ICommandRule>>()))
            .Callback<Type, ImmutableList<ICommandRule>>((_, rules) =>
            {
                registrationCalls++;
                capturedRules = rules;
            });

        var loader = new GameCommandValidatorLoader(
            accessor.Object,
            factory.Object,
            validator.Object);

        // Act
        await loader.LoadAsync(gameId, token);

        // Assert
        registrationCalls.Should().Be(1);
        factoryCalls.Should().Be(1);
        capturedRules.Should().Equal(expectedRule);
    }

    [Fact(DisplayName = "Cannot load command validations after cancellation or access any dependencies.")]
    [Trait("Category", "Unit")]
    public async Task CanNotLoadAfterCancellationAsync()
    {
        // Arrange
        using var cancellation = CancellationTokenSource.CreateLinkedTokenSource(TestContext.Current.CancellationToken);
        cancellation.Cancel();

        var loader = new GameCommandValidatorLoader(
            Mock.Of<IGameContextAccessor<IGameProfileContext>>(MockBehavior.Strict),
            Mock.Of<ICommandRuleFactory>(MockBehavior.Strict),
            Mock.Of<ICommandValidator>(MockBehavior.Strict));

        // Act
        var exception = await Record.ExceptionAsync(() =>
            loader.LoadAsync(new(Guid.NewGuid()), cancellation.Token));

        // Assert
        exception.Should().BeAssignableTo<OperationCanceledException>();
    }

    [Fact(DisplayName = "Can propagate profile loading failures before creating command rules.")]
    [Trait("Category", "Unit")]
    public async Task CanPropagateProfileFailureAsync()
    {
        // Arrange
        var gameId = new GameId(Guid.NewGuid());
        var token = TestContext.Current.CancellationToken;

        var failure = new InvalidOperationException("Profile unavailable.");

        var accessor = new Mock<IGameContextAccessor<IGameProfileContext>>(MockBehavior.Strict);
        accessor.Setup(x => x.GetContextAsync(gameId, token))
            .ThrowsAsync(failure);

        var loader = new GameCommandValidatorLoader(
            accessor.Object,
            Mock.Of<ICommandRuleFactory>(MockBehavior.Strict),
            Mock.Of<ICommandValidator>(MockBehavior.Strict));

        // Act
        var exception = await Record.ExceptionAsync(() =>
            loader.LoadAsync(gameId, token));

        // Assert
        exception.Should().BeSameAs(failure);
    }
}
