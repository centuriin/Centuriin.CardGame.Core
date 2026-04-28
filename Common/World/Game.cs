using System.ComponentModel;

using Centuriin.CardGame.Core.Common.Commands;
using Centuriin.CardGame.Core.Common.Events;
using Centuriin.CardGame.Core.Common.Observability;

namespace Centuriin.CardGame.Core.Common.World;

public sealed class Game : IGame
{
    private readonly ICommandValidator _commandValidator;
    private readonly IGameEventApplier _eventApplier;

    public GameId Id { get; }

    public GameStatus Status { get; private set; }

    public IGameState State { get; }

    public Game(
        GameId id,
        GameStatus gameStatus,
        IGameState gameState,
        ICommandValidator commandValidator,
        IGameEventApplier eventApplier)
    {
        Id = id;

        if (!Enum.IsDefined(gameStatus))
        {
            throw new InvalidEnumArgumentException(
                nameof(gameStatus),
                (int)gameStatus,
                typeof(GameStatus));
        }
        Status = gameStatus;

        ArgumentNullException.ThrowIfNull(gameState);
        State = gameState;

        ArgumentNullException.ThrowIfNull(commandValidator);
        _commandValidator = commandValidator;

        ArgumentNullException.ThrowIfNull(eventApplier);
        _eventApplier = eventApplier;
    }

    public async Task<ICommandResult> ExecuteAsync(ICommand command, CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(command);

        token.ThrowIfCancellationRequested();

        using var _ = Telemetry.StartActivity(command);

        var @event = _commandValidator.Validate(State, command);

        if (@event is null)
        {
            // todo fail command
            return null!;
        }

        var eventUnit = _eventApplier.ApplyAsync(@event, token);

        // todo make result
        return null!;
    }

    public async Task StartAsync(CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        if (Status is not GameStatus.Pending)
        {
            throw new InvalidOperationException();
        }

        using var _ = Telemetry.StartGameActivity(this);

        var unit = await _eventApplier.ApplyAsync(new GameStartedEvent(Id), token);

        Status = GameStatus.Running;
    }
}