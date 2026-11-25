using Centuriin.CardGame.Core.Engine.Commands.Rules;
using Centuriin.CardGame.Core.Engine.Loading.Configuration;
using Centuriin.CardGame.Core.Engine.Loading.Configuration.Commands;
using Centuriin.CardGame.Core.Engine.Loading.Configuration.Pipeline;

using FluentAssertions;

using Moq;

using Xunit;

namespace Centuriin.CardGame.Core.Engine.Tests.Loading.Configuration;

public sealed class DefaultProfileTests
{
    [Fact(DisplayName = "Can apply the active-player rule to registered commands.")]
    [Trait("Category", "Unit")]
    public void CanConfigureGeneralCommandRule()
    {
        // Arrange
        var registrationCalls = 0;
        var builder = new Mock<IConfigurableCommandValidator>(MockBehavior.Strict);
        builder.Setup(x => x.AddGeneralRule<ActorIsActivePlayerRule>())
            .Callback(() => registrationCalls++)
            .Returns(builder.Object);

        // Act
        DefaultProfile.Instance.Configure(builder.Object);

        // Assert
        registrationCalls.Should().Be(1);
    }

    [Fact(DisplayName = "Cannot configure a default pipeline without a builder.")]
    [Trait("Category", "Unit")]
    public void CanNotConfigureWithoutPipeline()
    {
        // Act
        var exception = Record.Exception(() =>
            DefaultProfile.Instance.Configure((IConfigurableGamePipeline)null!));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = "Cannot configure default command rules without a builder.")]
    [Trait("Category", "Unit")]
    public void CanNotConfigureWithoutCommandValidator()
    {
        // Act
        var exception = Record.Exception(() =>
            DefaultProfile.Instance.Configure((IConfigurableCommandValidator)null!));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }
}
