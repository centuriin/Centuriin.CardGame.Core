using Centuriin.CardGame.Core.Engine;
using Centuriin.CardGame.Core.Engine.Context;
using Centuriin.CardGame.Core.Engine.Loading.Configuration;

namespace Centuriin.CardGame.Core.Application.Context;

public sealed class GameProfileContextAccessor :
    IGameContextAccessor<IGameProfileContext>
{
    private readonly IGameProfilesCache _profilesRepository;

    public GameProfileContextAccessor(IGameProfilesCache profilesRepository)
    {
        ArgumentNullException.ThrowIfNull(profilesRepository);
        _profilesRepository = profilesRepository;
    }

    public async Task<IGameProfileContext> GetContextAsync(
        GameId gameId,
        CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        var profile = await _profilesRepository.GetProfileAsync(gameId, token);

        return new Context
        {
            GameId = gameId,
            Profile = profile
        };
    }

    private sealed class Context : IGameProfileContext
    {
        public required GameId GameId { get; init; }

        public required IGameProfile Profile { get; init; }
    }
}
