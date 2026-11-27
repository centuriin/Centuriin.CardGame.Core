using Centuriin.CardGame.Core.Application.Initialization;

using Microsoft.Extensions.DependencyInjection;

namespace Centuriin.CardGame.Core.Extensions.DependencyInjection;

/// <summary>
/// Registers scoped entity initializers under the current module key.
/// </summary>
internal sealed class ConfigurableEntityInitializers : IConfigurableEntityInitializers
{
    private readonly IServiceCollection _services;
    private readonly string _moduleKey;
    private readonly HashSet<Type> _types = [];

    public ConfigurableEntityInitializers(IServiceCollection services, string moduleKey)
    {
        _services = services;
        _moduleKey = moduleKey;
    }

    /// <inheritdoc/>
    public IConfigurableEntityInitializers Add<TInitializer>()
        where TInitializer : class, IEntityInitializer
    {
        if (!_types.Add(typeof(TInitializer)))
        {
            throw new InvalidOperationException(
                $"Entity initializer {typeof(TInitializer).Name} is already declared for module {_moduleKey}.");
        }

        _services.AddKeyedScoped<IEntityInitializer, TInitializer>(_moduleKey);
        return this;
    }
}
