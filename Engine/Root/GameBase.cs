using Centuriin.CardGame.Core.Engine.Commands;
using Centuriin.CardGame.Core.Engine.Events;
using Centuriin.CardGame.Core.Engine.Events.Application;
using Centuriin.CardGame.Core.Engine.Events.Domain;
using Centuriin.CardGame.Core.Engine.Root.State;

namespace Centuriin.CardGame.Core.Engine.Root;

public abstract class GameBase : IGame
{
    private readonly IGameState _state;
    private readonly IGameEventApplier _eventApplier;
    private readonly ICommandValidator _commandValidator;

    public GameId Id { get; }

    public GameVersion Version { get; protected set; } = GameVersion.Default;

    public IReadOnlyGameState State => _state;

    protected GameBase(
        GameId id,
        IGameState state,
        IGameEventApplier eventApplier,
        ICommandValidator commandValidator)
    {
        Id = id;

        ArgumentNullException.ThrowIfNull(state);
        _state = state;

        ArgumentNullException.ThrowIfNull(commandValidator);
        _commandValidator = commandValidator;

        ArgumentNullException.ThrowIfNull(eventApplier);
        _eventApplier = eventApplier;
    }

    public IGameApplicationEvent Start()
    {
        if (State.Status is not GameStatus.Created)
        {
            throw new InvalidOperationException(
                $"Game status should be {GameStatus.Created}," +
                $" but found {State.Status}.");
        }

        _state.ChangeStatus(GameStatus.Running);

        return StartCore();
    }

    public void Apply(IGameEventsBatch batch)
    {
        ArgumentNullException.ThrowIfNull(batch);

        ThrowIfNotRunning();

        foreach (var @event in batch)
        {
            _eventApplier.Apply(@event);
        }

        Version = batch.CurrentGameVersion;
    }

    public IGameApplicationEvent Execute(ICommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        ThrowIfNotRunning();

        if (!_commandValidator.IsValid(command))
        {
            return new GameCommandDeclinedEvent(Id, Version, command);
        }

        return ExecuteCore(command);
    }

    protected abstract IGameApplicationEvent StartCore();

    protected abstract IGameApplicationEvent ExecuteCore(ICommand command);

    private void ThrowIfNotRunning()
    {
        if (State.Status is not GameStatus.Running)
        {
            throw new InvalidOperationException(
                $"Game status should be {GameStatus.Running}," +
                $" but found {State.Status}.");
        }
    }
}
