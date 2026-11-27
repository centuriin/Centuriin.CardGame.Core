using Centuriin.CardGame.Core.Engine;
using Centuriin.CardGame.Core.Engine.Context;
using Centuriin.CardGame.Core.Engine.Loading.Configuration;
using Centuriin.CardGame.Core.Infrastructure.Storage.Abstractions.Repositories;

namespace Centuriin.CardGame.Core.Application.Context;

/// <summary>
/// Loads the profile associated with a game.
/// </summary>
public sealed class GameProfileContextAccessor :
    IGameContextAccessor<IGameProfileContext>
{
    private readonly IGameDescriptionRepository _descriptionRepository;
    private readonly IGameModulesCache _profilesRepository;

    /// <summary>
    /// Initializes a new <see cref="GameProfileContextAccessor"/> instance.
    /// </summary>
    /// <param name="descriptionRepository">
    /// The repository used to find the game's type.
    /// </param>
    /// <param name="profilesRepository">
    /// The cache used to resolve game profiles.
    /// </param>
    public GameProfileContextAccessor(
        IGameDescriptionRepository descriptionRepository,
        IGameModulesCache profilesRepository)
    {
        ArgumentNullException.ThrowIfNull(descriptionRepository);
        _descriptionRepository = descriptionRepository;

        ArgumentNullException.ThrowIfNull(profilesRepository);
        _profilesRepository = profilesRepository;
    }

    /// <inheritdoc/>
    public async Task<IGameProfileContext> GetContextAsync(
        GameId gameId,
        CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        var description = await _descriptionRepository.FindAsync(gameId, token)
            ?? throw new InvalidOperationException(
                $"Can't find description for game {gameId.Value}.");

        var module = await _profilesRepository.GetModuleAsync(
            description.GameTypeId, 
            token);

        return new Context
        {
            GameId = gameId,
            Profile = module.GameProfile
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
