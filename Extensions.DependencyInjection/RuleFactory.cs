using Centuriin.CardGame.Core.Engine.Commands.Rules;

using Microsoft.Extensions.DependencyInjection;

namespace Centuriin.CardGame.Core.Extensions.DependencyInjection;

/// <summary>
/// Creates command rules through the current service provider.
/// </summary>
internal sealed class RuleFactory : ICommandRuleFactory
{
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Initializes a new <see cref="RuleFactory"/> instance.
    /// </summary>
    /// <param name="serviceProvider">
    /// The service provider used to create dependencies.
    /// </param>
    public RuleFactory(IServiceProvider serviceProvider)
    {
        ArgumentNullException.ThrowIfNull(serviceProvider);
        _serviceProvider = serviceProvider;
    }

    /// <inheritdoc/>
    public TRule Create<TRule>() where TRule : ICommandRule =>
        ActivatorUtilities.CreateInstance<TRule>(_serviceProvider);
}
