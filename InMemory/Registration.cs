using Centuriin.CardGame.Core.Engine.ECS.Templates;
using Centuriin.CardGame.Core.Infrastructure.Storage.Abstractions.Repositories;
using Centuriin.CardGame.Core.Infrastructure.Storage.Abstractions.Repositories.InMemory;

using Microsoft.Extensions.DependencyInjection;

namespace Centuriin.CardGame.Core.Infrastructure.Storage.InMemory;

/// <summary>
/// Registers the in-memory template storage services.
/// </summary>
public static class Registration
{
    /// <summary>
    /// Registers the demonstration in-memory zone template repository.
    /// </summary>
    /// <param name="services">
    /// The service collection to configure.
    /// </param>
    /// <returns>
    /// The service collection for further configuration.
    /// </returns>
    /// <remarks>
    /// Deck, game description, event, and snapshot repositories must be supplied separately.
    /// </remarks>
    public static IServiceCollection UseInMemoryStorage(this IServiceCollection services) =>
        services
            .AddSingleton<
                ITemplatesRepository<ZoneTemplate>,
                ZoneTemplatesRepository>();
}
