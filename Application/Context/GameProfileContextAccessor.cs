using Centuriin.CardGame.Core.Engine;
using Centuriin.CardGame.Core.Engine.Context;
using Centuriin.CardGame.Core.Engine.Loading.Configuration;

namespace Centuriin.CardGame.Core.Application.Context;

/// <summary>
/// Loads the profile associated with a game.
/// </summary>
public sealed class GameProfileContextAccessor :
    IGameContextAccessor<IGameProfileContext>
{
    private readonly IGameProfilesCache _profilesRepository;

    /// <summary>
    /// Initializes a new <see cref="GameProfileContextAccessor"/> instance.
    /// </summary>
    /// <param name="profilesRepository">
    /// The cache used to resolve game profiles.
    /// </param>
    public GameProfileContextAccessor(IGameProfilesCache profilesRepository)
    {
        ArgumentNullException.ThrowIfNull(profilesRepository);
        _profilesRepository = profilesRepository;
    }

    /// <inheritdoc/>
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

    /// <summary>
    /// Carries the profile selected for a game.
    /// </summary>
    private sealed class Context : IGameProfileContext
    {
        /// <inheritdoc/>
        public required GameId GameId { get; init; }

        /// <inheritdoc/>
        public required IGameProfile Profile { get; init; }
    }
}
