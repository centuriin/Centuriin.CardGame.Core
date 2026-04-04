using Centuriin.CardGame.Core.Common.Components;
using Centuriin.CardGame.Core.Common.Components.Players;
using Centuriin.CardGame.Core.Common.Entities.Players;

namespace Centuriin.CardGame.Core.Common.Loaders;

public sealed class ClassicPlayersLoader : IPlayersLoader
{
    private readonly IGameState _gameState;

    public ClassicPlayersLoader(IGameState gameState)
    {
        ArgumentNullException.ThrowIfNull(gameState);
        _gameState = gameState;
    }

    public async Task LoadAsync(IReadOnlyCollection<PlayerId> playerIds, CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        _gameState.AddEntity<Player, PlayerId>(Player.System);

        foreach (var id in playerIds)
        {
            var player = new Player(id);
            player.Add(new PlayerRoleComponent(PlayerRole.Participant));

            _gameState.AddEntity<Player, PlayerId>(player);
        }
    }
}
