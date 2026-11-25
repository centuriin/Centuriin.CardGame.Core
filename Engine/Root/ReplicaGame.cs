using Centuriin.CardGame.Core.Engine.Commands;
using Centuriin.CardGame.Core.Engine.Events;
using Centuriin.CardGame.Core.Engine.Root.State;

namespace Centuriin.CardGame.Core.Engine.Root;

/// <summary>
/// Validates commands for external execution and applies authoritative event batches.
/// </summary>
public sealed class ReplicaGame : GameBase
{
    /// <summary>
    /// Initializes a new <see cref="ReplicaGame"/> instance.
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
    public ReplicaGame(
        GameId id,
        IGameState state,
        IGameEventApplier eventApplier,
        ICommandValidator commandValidator)
        : base(id, state, eventApplier, commandValidator)
    {
    }

    /// <inheritdoc/>
    protected override PreparedResult StartCore()
        => new(Version);

    /// <inheritdoc/>
    protected override PreparedResult ExecuteCore(ICommand command)
        => new(Version);
}
