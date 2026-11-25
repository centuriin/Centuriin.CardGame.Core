using Centuriin.CardGame.Core.Application;
using Centuriin.CardGame.Core.Engine;

using Microsoft.Extensions.DependencyInjection;

namespace Centuriin.CardGame.Core.Extensions.DependencyInjection;

/// <summary>
/// Resolves a keyed dependency using the game type's profile key.
/// </summary>
/// <typeparam name="TDependency">
/// The game-specific dependency type to resolve.
/// </typeparam>
internal sealed class GameModuleAdapter<TDependency> :
    IGameModuleAdapter<TDependency>
    where TDependency : notnull
{
    private readonly IGameProfilesCache _profilesCache;
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Initializes a new <see cref="GameModuleAdapter{TDependency}"/> instance.
    /// </summary>
    /// <param name="profilesRepository">
    /// The cache used to resolve game profiles.
    /// </param>
    /// <param name="serviceProvider">
    /// The service provider used to create dependencies.
    /// </param>
    public GameModuleAdapter(
        IGameProfilesCache profilesRepository,
        IServiceProvider serviceProvider)
    {
        ArgumentNullException.ThrowIfNull(profilesRepository);
        _profilesCache = profilesRepository;

        ArgumentNullException.ThrowIfNull(serviceProvider);
        _serviceProvider = serviceProvider;
    }

    /// <inheritdoc/>
    public async ValueTask<TDependency> GetDependencyAsync(
        GameTypeId gameTypeId,
        CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        var profile = await _profilesCache.GetProfileAsync(
            gameTypeId,
            token);

        return _serviceProvider.GetRequiredKeyedService<TDependency>(profile.Key);
    }
}
