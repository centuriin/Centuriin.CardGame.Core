using Centuriin.CardGame.Core.Common.Configuration;
using Centuriin.CardGame.Core.Common.World;

namespace Centuriin.CardGame.Core.Common.Storage.Repositories;

public interface IGameConfigurationRepository
{
    public ValueTask<IGameConfiguration?> FindAsync(GameTypeId gameTypeId, CancellationToken token);
}