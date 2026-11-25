using Centuriin.CardGame.Core.Application.Initialization;
using Centuriin.CardGame.Core.Drunkard.Initialization;

using Microsoft.Extensions.DependencyInjection;

namespace Centuriin.CardGame.Core.Drunkard;

/// <summary>
/// Registers the Drunkard game module.
/// </summary>
public static class Registration
{
    /// <summary>
    /// Registers the Drunkard-specific initializer under the module's profile key.
    /// </summary>
    /// <param name="services">
    /// The service collection to configure.
    /// </param>
    /// <returns>
    /// The service collection for further configuration.
    /// </returns>
    /// <remarks>
    /// The host must also associate the game type identifier with <see cref="GameProfile.Instance"/>.
    /// </remarks>
    public static IServiceCollection AddDrunkard(this IServiceCollection services)
        => services
            .AddKeyedScoped<IGameInitializer, GameInitializer>(
                GameProfile.KEY);
}
