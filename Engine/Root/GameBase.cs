using Centuriin.CardGame.Core.Engine.Commands;
using Centuriin.CardGame.Core.Engine.Events;
using Centuriin.CardGame.Core.Engine.Events.Domain;
using Centuriin.CardGame.Core.Engine.Root.State;

namespace Centuriin.CardGame.Core.Engine.Root;

/// <summary>
/// Validates lifecycle, identifiers, and versions before delegating game operations.
/// </summary>
public abstract class GameBase : IGame
{
    private const int VERSION_INCREMENT = 1;

    private readonly IGameState _state;
    private readonly IGameEventApplier _eventApplier;
    private readonly ICommandValidator _commandValidator;

    /// <inheritdoc/>
    public GameId Id { get; }

    /// <inheritdoc/>
    public GameVersion Version { get; protected set; } = GameVersion.Default;

    /// <inheritdoc/>
    public IReadOnlyGameState State => _state;

    /// <summary>
    /// Initializes a new <see cref="GameBase"/> instance.
    /// </summary>
    /// <param name="id">
    /// The game identifier.
    /// </param>
    /// <param name="state">
    /// The mutable game state.
    /// </param>
    /// <param name="eventApplier">
    /// The service used to apply recorded events.
    /// </param>
    /// <param name="commandValidator">
    /// The validator used to check commands before execution.
    /// </param>
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

    /// <inheritdoc/>
    public IEngineResult Start()
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

    /// <inheritdoc/>
    public void Apply(IGameEventsBatch batch)
    {
        ArgumentNullException.ThrowIfNull(batch);

        if (batch.GameVersion.Value - Version.Value != VERSION_INCREMENT)
        {
            throw new InvalidOperationException(
                $"Batch version {batch.GameVersion.Value} should be greater than" +
                $" current version {Version.Value}");
        }

        ThrowIfNotRunning();

        foreach (var @event in batch)
        {
            _eventApplier.Apply(@event);
        }

        Version = batch.GameVersion;
    }

    /// <inheritdoc/>
    public IEngineResult Execute(ICommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        if (command.GameId != Id)
        {
            throw new InvalidOperationException(
                $"Command game id {command.GameId.Value} should be equal to" +
                $" current game id {Id.Value}");
        }

        ThrowIfNotRunning();

        if (!_commandValidator.IsValid(command))
        {
            return new RejectedResult(Version);
        }

        return ExecuteCore(command);
    }

    /// <summary>
    /// Performs the startup operation specific to the authoritative game or replica.
    /// </summary>
    /// <returns>
    /// The execution, preparation, or rejection result and its game version.
    /// </returns>
    protected abstract IEngineResult StartCore();

    /// <summary>
    /// Performs command processing specific to the authoritative game or replica.
    /// </summary>
    /// <param name="command">
    /// The command to validate or execute.
    /// </param>
    /// <returns>
    /// The execution, preparation, or rejection result and its game version.
    /// </returns>
    protected abstract IEngineResult ExecuteCore(ICommand command);

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
