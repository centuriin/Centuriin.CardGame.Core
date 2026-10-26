using Centuriin.CardGame.Core.Common.ECS.Systems;

using Microsoft.Extensions.DependencyInjection;

namespace Centuriin.CardGame.Core.Extensions.DependencyInjection;

internal sealed class SystemFactory : ISystemFactory
{
    private readonly IServiceProvider _serviceProvider;

    public SystemFactory(IServiceProvider serviceProvider)
    {
        ArgumentNullException.ThrowIfNull(serviceProvider);
        _serviceProvider = serviceProvider;
    }

    public TSystem Create<TSystem>()
        where TSystem : SystemBase
        => ActivatorUtilities.CreateInstance<TSystem>(_serviceProvider);
}