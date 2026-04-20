using Centuriin.CardGame.Core.Common.Factories;
using Centuriin.CardGame.Core.Common.Systems;

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

    public SystemBase Create(Type systemType)
    {
        ArgumentNullException.ThrowIfNull(systemType);

        if (!systemType.IsAssignableTo(typeof(SystemBase)))
        {
            throw new InvalidOperationException();
        }

        return (SystemBase)ActivatorUtilities.CreateInstance(_serviceProvider, systemType);
    }
        
}