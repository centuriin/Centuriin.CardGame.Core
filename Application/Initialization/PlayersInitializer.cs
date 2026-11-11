using Centuriin.CardGame.Core.Engine;
using Centuriin.CardGame.Core.Engine.Context;
using Centuriin.CardGame.Core.Engine.ECS.Components.Players;
using Centuriin.CardGame.Core.Engine.ECS.Entities;
using Centuriin.CardGame.Core.Engine.ECS.Entities.Players;

namespace Application.Initialization;

public sealed class PlayersInitializer : IGameEntityInitializer
{
    public Task InitializeAsync(
        GameDescription gameDescription,
        IInitialStateContext initialStateContext,
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(gameDescription);
        ArgumentNullException.ThrowIfNull(initialStateContext);

        token.ThrowIfCancellationRequested();

        var entityId = new EntityId(1);
        foreach (var id in gameDescription.PlayerIds)
        {
            if (id == PlayerId.System)
            {
                initialStateContext.Entities.AddEntity(Player.System);
                continue;
            }

            var player = new Player(entityId++);
            player.Add(
                new PlayerRoleComponent(PlayerRole.Participant),
                new PlayerIdentifierComponent(id));

            initialStateContext.Entities.AddEntity(player);
        }

        return Task.CompletedTask;
    }
}
