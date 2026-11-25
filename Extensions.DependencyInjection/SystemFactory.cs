using Centuriin.CardGame.Core.Engine.ECS.Systems;

using Microsoft.Extensions.DependencyInjection;

namespace Centuriin.CardGame.Core.Extensions.DependencyInjection;

/// <summary>
/// Creates game systems through the current service provider.
/// </summary>
internal sealed class SystemFactory : ISystemFactory
{
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Initializes a new <see cref="SystemFactory"/> instance.
    /// </summary>
    /// <param name="serviceProvider">
    /// The service provider used to create dependencies.
    /// </param>
    public SystemFactory(IServiceProvider serviceProvider)
    {
        ArgumentNullException.ThrowIfNull(serviceProvider);
        _serviceProvider = serviceProvider;
    }

    /// <inheritdoc/>
    public TSystem Create<TSystem>()
        where TSystem : ISystem
        => ActivatorUtilities.CreateInstance<TSystem>(_serviceProvider);
}