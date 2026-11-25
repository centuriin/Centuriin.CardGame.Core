using Centuriin.CardGame.Core.Engine.Commands;
using Centuriin.CardGame.Core.Engine.Commands.Rules;
using Centuriin.CardGame.Core.Engine.ECS.Components.Players;
using Centuriin.CardGame.Core.Engine.ECS.Entities;
using Centuriin.CardGame.Core.Engine.ECS.Entities.Players;
using Centuriin.CardGame.Core.Engine.Root.State;

using FluentAssertions;

using Moq;

using Xunit;

namespace Centuriin.CardGame.Core.Engine.Tests.Commands.Rules;

public sealed class ActorIsActivePlayerRuleTests
{
    [Theory(DisplayName = "Can validate a command by comparing its external actor identifier with the active entity.")]
    [InlineData(true)]
    [InlineData(false)]
    [Trait("Category", "Unit")]
    public void CanCheckActivePlayer(bool actorIsActive)
    {
        // Arrange
        var actor = new PlayerId(Guid.NewGuid());

        Player[] players = [
            new(new(1), [new PlayerIdentifierComponent(actor)]),
            new(new(2), [new PlayerIdentifierComponent(new(Guid.NewGuid()))])
        ];

        var command = new Mock<ICommand>(MockBehavior.Strict);
        command.SetupGet(x => x.Actor)
            .Returns(actor);

        var entities = new Mock<IReadOnlyEntitiesCollection>(MockBehavior.Strict);
        entities.Setup(x => x.Query<Player>())
            .Returns(players);

        var automat = new Mock<IReadOnlyTurnAutomat>(MockBehavior.Strict);
        automat.SetupGet(x => x.ActivePlayer)
            .Returns(new EntityId(actorIsActive ? 1 : 2));

        var state = new Mock<IReadOnlyGameState>(MockBehavior.Strict);
        state.SetupGet(x => x.Entities)
            .Returns(entities.Object);
        state.SetupGet(x => x.TurnAutomat)
            .Returns(automat.Object);

        ICommandRule rule = new ActorIsActivePlayerRule();

        // Act
        var result = rule.Check(command.Object, state.Object);

        // Assert
        result.Should().Be(actorIsActive);
    }

    [Fact(DisplayName = "Cannot check the active player without game state.")]
    [Trait("Category", "Unit")]
    public void CanNotCheckWithoutGameState()
    {
        // Arrange
        var command = Mock.Of<ICommand>(MockBehavior.Strict);

        var rule = new ActorIsActivePlayerRule();

        // Act
        var exception = Record.Exception(() =>
            rule.Check(command, null!));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }
}
