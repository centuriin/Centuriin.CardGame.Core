using Centuriin.CardGame.Core.Application.Initialization;
using Centuriin.CardGame.Core.Application.Modulation;

using Microsoft.Extensions.DependencyInjection;

namespace Centuriin.CardGame.Core.Extensions.DependencyInjection;

/// <summary>
/// Registers ordinary module dependencies without service keys.
/// </summary>
internal sealed class ConfigurableModuleDependencies : IConfigurableModuleDependencies
{
    private readonly IServiceCollection _services;

    public ConfigurableModuleDependencies(IServiceCollection services)
    {
        _services = services;
    }

    /// <inheritdoc/>
    public IConfigurableModuleDependencies Add<TDependency>()
        where TDependency : class
        => Add<TDependency, TDependency>();

    /// <inheritdoc/>
    public IConfigurableModuleDependencies Add<TService, TImplementation>()
        where TService : class
        where TImplementation : class, TService
    {
        if (typeof(IEntityInitializer).IsAssignableFrom(typeof(TImplementation)))
        {
            throw new InvalidOperationException("Declare entity initializers through IConfigurableEntityInitializers.");
        }

        _services.AddScoped<TService, TImplementation>();
        return this;
    }
}
