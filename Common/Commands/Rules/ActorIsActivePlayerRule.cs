using Centuriin.CardGame.Core.Common.ECS.Components.Players;
using Centuriin.CardGame.Core.Common.ECS.Entities.Players;
using Centuriin.CardGame.Core.Common.World;

namespace Centuriin.CardGame.Core.Common.Commands.Rules;

public sealed class ActorIsActivePlayerRule : IGameRule
{
    public bool Check(PlayerId actor, IGameState gameState)
    {
        ArgumentNullException.ThrowIfNull(gameState);

        var actorEntityId = gameState.Query<Player>()
            .WithComponent<PlayerIdentifierComponent>(x => x.PlayerId == actor)
            .Select(x => x.Id)
            .Single();

        if (gameState.TurnAutomat.ActivePlayer == actorEntityId)
        {
            return true;
        }

        return false;
    }
}
