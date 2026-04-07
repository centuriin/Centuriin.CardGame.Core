using Centuriin.CardGame.Core.Common.Components.Players;
using Centuriin.CardGame.Core.Common.Entities.Players;

namespace Centuriin.CardGame.Core.Common.Loaders;

public sealed class ClassicPlayersLoader : IGameLoader
{
    public Task LoadAsync(GameSetup setup, IGameState gameState, CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(setup);
        ArgumentNullException.ThrowIfNull(gameState);

        token.ThrowIfCancellationRequested();

        gameState.AddEntity<Player, PlayerId>(Player.System);

        foreach (var id in setup.PlayerIds)
        {
            var player = new Player(id);
            player.Add(new PlayerRoleComponent(PlayerRole.Participant));

            gameState.AddEntity<Player, PlayerId>(player);
        }

        return Task.CompletedTask;
    }
}
