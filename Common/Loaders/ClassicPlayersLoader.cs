using Centuriin.CardGame.Core.Common.Components.Players;
using Centuriin.CardGame.Core.Common.Entities.Players;
using Centuriin.CardGame.Core.Common.World;

namespace Centuriin.CardGame.Core.Common.Loaders;

public sealed class ClassicPlayersLoader : IGameLoader
{
    public Task LoadAsync(GameSetup setup, IGameState gameState, CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(setup);
        ArgumentNullException.ThrowIfNull(gameState);

        token.ThrowIfCancellationRequested();

        gameState.AddEntity(Player.System);

        var index = 1;
        foreach (var id in setup.PlayerIds)
        {
            var player = new Player(new(index++));
            player.Add(new PlayerRoleComponent(PlayerRole.Participant));

            gameState.AddEntity(player);
        }

        return Task.CompletedTask;
    }
}
