using Centuriin.CardGame.Core.Common.Entities.Players;

namespace Centuriin.CardGame.Core.Common.Loaders;

public interface IPlayersLoader
{
    public Task LoadAsync(IReadOnlyCollection<PlayerId> playerIds, CancellationToken token);
}