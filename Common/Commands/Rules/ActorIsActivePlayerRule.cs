using Centuriin.CardGame.Core.Common.Components.Players;
using Centuriin.CardGame.Core.Common.Entities.Players;
using Centuriin.CardGame.Core.Common.World;

namespace Centuriin.CardGame.Core.Common.Commands.Rules;

public sealed class ActorIsActivePlayerRule : IGameRule
{
    public RuleResult Check(IGameState gameState, ICommand command)
    {
        ArgumentNullException.ThrowIfNull(gameState);
        ArgumentNullException.ThrowIfNull(command);

        var actorEntityId = gameState.Query<Player>()
            .WithComponent<PlayerIdentifierComponent>(x => x.PlayerId == command.Actor)
            .Select(x => x.Id)
            .Single();

        if (gameState.TurnAutomat.ActivePlayer == actorEntityId)
        {
            return RuleResult.Success();
        }

        return RuleResult.Failure(
            $"Active player id {gameState.TurnAutomat.ActivePlayer.Value}" +
            $" and actor player id {actorEntityId.Value} doesn't equals.");
    }
}
