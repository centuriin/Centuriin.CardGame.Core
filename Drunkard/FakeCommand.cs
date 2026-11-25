using Centuriin.CardGame.Core.Engine;
using Centuriin.CardGame.Core.Engine.Commands;
using Centuriin.CardGame.Core.Engine.Events.Domain;

namespace Centuriin.CardGame.Core.Drunkard;

/// <summary>
/// Triggers the game-start event pipeline for demonstration purposes.
/// </summary>
/// <param name="GameId">
/// The identifier of the associated game.
/// </param>
/// <param name="Actor">
/// The player who issued the command.
/// </param>
public sealed record class FakeCommand(GameId GameId, PlayerId Actor) : ICommand
{
    /// <inheritdoc/>
    public IGameEvent YieldGameEvent() => new GameStartedEvent(GameId);
}