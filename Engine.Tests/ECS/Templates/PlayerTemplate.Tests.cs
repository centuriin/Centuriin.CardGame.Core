using Centuriin.CardGame.Core.Engine.ECS.Components.Players;
using Centuriin.CardGame.Core.Engine.ECS.Entities.Players;
using Centuriin.CardGame.Core.Engine.ECS.Templates;

using FluentAssertions;

using Xunit;

namespace Centuriin.CardGame.Core.Engine.Tests.ECS.Templates;

public sealed class PlayerTemplateTests
{
    [Fact(DisplayName = "Can create a default participant template for an external player identifier.")]
    [Trait("Category", "Unit")]
    public void CanCreateDefaultParticipantTemplate()
    {
        // Arrange
        var playerId = new PlayerId(Guid.NewGuid());

        // Act
        var template = PlayerTemplate.CreateDefault(playerId);

        // Assert
        template.Components.Should().HaveCount(2);
        template.Components.OfType<PlayerIdentifierComponent>().Single().PlayerId.Should().Be(playerId);
        template.Components.OfType<PlayerRoleComponent>().Single().Role.Should().Be(PlayerRole.Participant);
    }
}
