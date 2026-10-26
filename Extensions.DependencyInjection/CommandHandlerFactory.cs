using Centuriin.CardGame.Core.Common.Commands;
using Centuriin.CardGame.Core.Common.World;

using Microsoft.Extensions.DependencyInjection;

namespace Centuriin.CardGame.Core.Extensions.DependencyInjection;

public sealed class CommandHandlerFactory : ICommandHandlerFactory
{
    private readonly IServiceProvider _serviceProvider;

    public CommandHandlerFactory(IServiceProvider serviceProvider)
    {
        ArgumentNullException.ThrowIfNull(serviceProvider);
        _serviceProvider = serviceProvider;
    }

    /// <inheritdoc/>
    public ICommandHandler Create(IGame game) =>
        ActivatorUtilities.CreateInstance<CommandHandler>(
            _serviceProvider,
            game);
}
