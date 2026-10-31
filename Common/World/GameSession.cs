using Centuriin.CardGame.Core.Common.Commands;
using Centuriin.CardGame.Core.Common.Observability;
using Centuriin.CardGame.Core.Common.Root;

namespace Centuriin.CardGame.Core.Common.World;

// Todo: create other DI collection for game 
public sealed class GameSession : IGameSession
{
    private readonly IGame _game;
    private readonly ICommandValidator _validator;

    public GameId Id => _game.Id;

    public GameTypeId GameTypeId => _game.TypeId;

    public GameSession(
        IGame game,
        ICommandValidator validator)
    {
        ArgumentNullException.ThrowIfNull(game);
        _game = game;

        ArgumentNullException.ThrowIfNull(validator);
        _validator = validator;
    }

    /// <inheritdoc/>
    public async Task StartAsync(CancellationToken token) =>
        await _game.StartAsync(token);

    /// <inheritdoc/>
    public async Task<CommandResult> ExecuteAsync(ICommand command, CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(command);

        token.ThrowIfCancellationRequested();

        using var _ = Telemetry.StartActivity(command);

        if (_validator.IsValid(command))
        {
            await _game.ExecuteAsync(command, token);

            return new(true, _game.Version);
        }

        return new(false, _game.Version);
    }

    /// <inheritdoc/>
    public async Task AbortAsync(CancellationToken token) =>
        await _game.AbortAsync(token);
}
