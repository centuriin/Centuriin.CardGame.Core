using Centuriin.CardGame.Core.Common.ECS.Components.Players;
using Centuriin.CardGame.Core.Common.ECS.Entities.Players;
using Centuriin.CardGame.Core.Common.Root.State;

namespace Centuriin.CardGame.Core.Common.Commands.Rules;

public sealed class ActorIsActivePlayerRule : ICommandRule<ICommand>
{
    public bool Check(ICommand command, IReadOnlyGameState gameState)
    {
        ArgumentNullException.ThrowIfNull(gameState);

        var actorEntityId = gameState.Entities
            .Query<Player>()
            .WithComponent<PlayerIdentifierComponent>(x => x.PlayerId == command.Actor)
            .Select(x => x.Id)
            .Single();

        if (gameState.TurnAutomat.ActivePlayer == actorEntityId)
        {
            return true;
        }

        return false;
    }
}
