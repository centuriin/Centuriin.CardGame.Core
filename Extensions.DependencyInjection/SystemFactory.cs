using Centuriin.CardGame.Core.Engine.ECS.Systems;

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
        where TSystem : ISystem
        => ActivatorUtilities.CreateInstance<TSystem>(_serviceProvider);
}