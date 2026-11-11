using Centuriin.CardGame.Core.Engine.ECS.Templates;
using Centuriin.CardGame.Core.Infrastructure.Storage.Abstractions.Repositories;
using Centuriin.CardGame.Core.Infrastructure.Storage.Abstractions.Repositories.InMemory;

using Microsoft.Extensions.DependencyInjection;

namespace Centuriin.CardGame.Core.Infrastructure.Storage.InMemory;

public static class Registration
{
    public static IServiceCollection UseInMemoryStorage(this IServiceCollection services) =>
        services
            .AddSingleton<
                ITemplatesRepository<ZoneTemplate>,
                ZoneTemplatesRepository>();
}
