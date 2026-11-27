using Centuriin.CardGame.Core.Application;
using Centuriin.CardGame.Core.Application.Modulation;
using Centuriin.CardGame.Core.Engine;

using Microsoft.Extensions.DependencyInjection;

namespace Centuriin.CardGame.Core.Extensions.DependencyInjection;

/// <summary>
/// Resolves a keyed dependency using the game type's profile key.
/// </summary>
/// <typeparam name="TDependency">
/// The game-specific dependency type to resolve.
/// </typeparam>
internal sealed class ModuleAccessor<TDependency> : IModulable<TDependency>
    where TDependency : notnull
{
    private readonly IGameModulesCache _profilesCache;
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Initializes a new <see cref="ModuleAccessor{TDependency}"/> instance.
    /// </summary>
    /// <param name="profilesRepository">
    /// The cache used to resolve game profiles.
    /// </param>
    /// <param name="serviceProvider">
    /// The service provider used to create dependencies.
    /// </param>
    public ModuleAccessor(
        IGameModulesCache profilesRepository,
        IServiceProvider serviceProvider)
    {
        ArgumentNullException.ThrowIfNull(profilesRepository);
        _profilesCache = profilesRepository;

        ArgumentNullException.ThrowIfNull(serviceProvider);
        _serviceProvider = serviceProvider;
    }

    /// <inheritdoc/>
    public async ValueTask<TDependency> ResolveSingleAsync(
        GameTypeId gameTypeId,
        CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        var profile = await _profilesCache.GetModuleAsync(
            gameTypeId,
            token);

        return _serviceProvider.GetRequiredKeyedService<TDependency>(profile.Key);
    }

    /// <inheritdoc/>
    public async ValueTask<IEnumerable<TDependency>> ResolveCollectionAsync(
        GameTypeId gameTypeId,
        CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        var profile = await _profilesCache.GetModuleAsync(
            gameTypeId,
            token);

        return _serviceProvider.GetKeyedServices<TDependency>(profile.Key);
    }
}
