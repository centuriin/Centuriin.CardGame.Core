using Centuriin.CardGame.Core.Common.Configuration;

namespace Centuriin.CardGame.Core.Common.Storage.Repositories;

public interface IGameConfigurationRepository
{
    public ValueTask<IGameConfiguration?> FindAsync(GameTypeId gameTypeId, CancellationToken token);
}