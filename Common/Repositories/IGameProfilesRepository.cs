using Centuriin.CardGame.Core.Common.GameProfiles;

namespace Centuriin.CardGame.Core.Common.Repositories;

public interface IGameProfilesRepository
{
    public ValueTask<IEnumerable<IGameProfile>> GetGameProfilesAsync(CancellationToken token);
}