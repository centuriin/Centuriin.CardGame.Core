using Centuriin.CardGame.Core.Common.Components.Players;
using Centuriin.CardGame.Core.Common.Entities;
using Centuriin.CardGame.Core.Common.Entities.Players;
using Centuriin.CardGame.Core.Common.World;

namespace Centuriin.CardGame.Core.Common.Loaders;

public sealed class PlayersLoader : IGameLoader
{
    public Task LoadAsync(GameSetup setup, IGameState gameState, CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(setup);
        ArgumentNullException.ThrowIfNull(gameState);

        token.ThrowIfCancellationRequested();

        var entityId = new EntityId(1);
        foreach (var id in setup.PlayerIds)
        {
            if (id == PlayerId.System)
            {
                gameState.AddEntity(Player.System);
                continue;
            }

            var player = new Player(entityId++);
            player.Add(
                new PlayerRoleComponent(PlayerRole.Participant),
                new PlayerIdentifierComponent(id));

            gameState.AddEntity(player);
        }

        return Task.CompletedTask;
    }
}
