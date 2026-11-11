using Centuriin.CardGame.Core.Engine;
using Centuriin.CardGame.Core.Engine.Loading.Configuration;

namespace Centuriin.CardGame.Core.Infrastructure.Storage.Abstractions.Repositories;

public interface IGameProfilesRepository
{
    public ValueTask<IGameProfile> GetProfileAsync(
        GameId gameId,
        CancellationToken token);
}