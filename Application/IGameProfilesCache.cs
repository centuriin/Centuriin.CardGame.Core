using Centuriin.CardGame.Core.Engine;
using Centuriin.CardGame.Core.Engine.Loading.Configuration;

namespace Centuriin.CardGame.Core.Application;

public interface IGameProfilesCache
{
    public ValueTask<IGameProfile> GetProfileAsync(
        GameId gameId,
        CancellationToken token);

    public ValueTask<IGameProfile> GetProfileAsync(
        GameTypeId gameTypeId,
        CancellationToken token);
}