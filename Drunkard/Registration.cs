using Centuriin.CardGame.Core.Application.Initialization;
using Centuriin.CardGame.Core.Drunkard.Initialization;

using Microsoft.Extensions.DependencyInjection;

namespace Centuriin.CardGame.Core.Drunkard;

public static class Registration
{
    public static IServiceCollection AddDrunkard(this IServiceCollection services)
        => services
            .AddKeyedScoped<IGameInitializer, GameInitializer>(
                GameProfile.KEY);
}
