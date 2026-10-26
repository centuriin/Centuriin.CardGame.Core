using Centuriin.CardGame.Core.Common.Observability;
using Centuriin.CardGame.Core.Common.World;

namespace Centuriin.CardGame.Core.Common.Commands;

public sealed class CommandExecutor : ICommandExecutor
{
    private readonly IGame _game;
    private readonly ICommandValidator _validator;

    public CommandExecutor(IGame game, ICommandValidator validator)
    {
        ArgumentNullException.ThrowIfNull(game);
        _game = game;

        ArgumentNullException.ThrowIfNull(validator);
        _validator = validator;
    }

    /// <inheritdoc/>
    public async Task<CommandResult> ExecuteAsync(ICommand command, CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(command);

        token.ThrowIfCancellationRequested();

        using var _ = Telemetry.StartActivity(command);

        if (_validator.IsValid(command))
        {
            await _game.ApplyAsync(command.YieldDomainEvent(), token);
            return new CommandResult(true, _game.Version);
        }

        return new CommandResult(false, _game.Version);
    }
}
