using Centuriin.CardGame.Core.Engine;
using Centuriin.CardGame.Core.Engine.Context;
using Centuriin.CardGame.Core.Engine.Loading.Configuration;
using Centuriin.CardGame.Core.Infrastructure.Storage.Abstractions.Repositories;

namespace Centuriin.CardGame.Core.Application.Context;

public sealed class GameProfileContextAccessor :
    IGameContextAccessor<IGameProfileContext>
{
    private readonly IGameProfilesRepository _profilesRepository;

    public GameProfileContextAccessor(IGameProfilesRepository profilesRepository)
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
