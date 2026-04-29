using Centuriin.CardGame.Core.Common.Commands.Rules;
using Centuriin.CardGame.Core.Common.Factories;

using Microsoft.Extensions.DependencyInjection;

namespace Centuriin.CardGame.Core.Extensions.DependencyInjection;

public sealed class RuleFactory : IRuleFactory
{
    private readonly IServiceProvider _serviceProvider;

    public RuleFactory(IServiceProvider serviceProvider)
    {
        ArgumentNullException.ThrowIfNull(serviceProvider);
        _serviceProvider = serviceProvider;
    }

    public TGameRule Create<TGameRule>() where TGameRule : IGameRule =>
        ActivatorUtilities.CreateInstance<TGameRule>(_serviceProvider);
}
