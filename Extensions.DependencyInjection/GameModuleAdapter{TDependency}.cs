using Centuriin.CardGame.Core.Application;
using Centuriin.CardGame.Core.Engine;

using Microsoft.Extensions.DependencyInjection;

namespace Centuriin.CardGame.Core.Extensions.DependencyInjection;

internal sealed class GameModuleAdapter<TDependency> :
    IGameModuleAdapter<TDependency>
    where TDependency : notnull
{
    private readonly IGameProfilesCache _profilesCache;
    private readonly IServiceProvider _serviceProvider;

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
