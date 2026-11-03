using Centuriin.CardGame.Core.Common.Context;
using Centuriin.CardGame.Core.Common.ECS.Components.Players;
using Centuriin.CardGame.Core.Common.ECS.Entities;
using Centuriin.CardGame.Core.Common.ECS.Entities.Players;
using Centuriin.CardGame.Core.Common.World;

namespace Centuriin.CardGame.Core.Common.Initialization;

public sealed class PlayersInitializer : IGameEntityInitializer
{
    public Task InitializeAsync(
        GameSetup setup,
        IInitialStateContext initialStateContext,
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(setup);
        ArgumentNullException.ThrowIfNull(initialStateContext);

        token.ThrowIfCancellationRequested();

        var entityId = new EntityId(1);
        foreach (var id in setup.PlayerIds)
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
