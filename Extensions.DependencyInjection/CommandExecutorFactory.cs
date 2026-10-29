using Centuriin.CardGame.Core.Common.Commands;
using Centuriin.CardGame.Core.Common.Root;

using Microsoft.Extensions.DependencyInjection;

namespace Centuriin.CardGame.Core.Extensions.DependencyInjection;

internal sealed class CommandExecutorFactory : ICommandExecutorFactory
{
    private readonly IServiceProvider _serviceProvider;

    public CommandExecutorFactory(IServiceProvider serviceProvider)
    {
        ArgumentNullException.ThrowIfNull(serviceProvider);
        _serviceProvider = serviceProvider;
    }

    /// <inheritdoc/>
    public ICommandExecutor Create(IGame game) =>
        ActivatorUtilities.CreateInstance<CommandExecutor>(
            _serviceProvider,
            game);
}
