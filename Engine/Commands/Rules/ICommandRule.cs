namespace Centuriin.CardGame.Core.Engine.Commands.Rules;

using Centuriin.CardGame.Core.Engine.Commands;
using Centuriin.CardGame.Core.Engine.Root.State;

/// <summary>
/// Checks whether a command is allowed in the current game state.
/// </summary>
public interface ICommandRule
{
    /// <summary>
    /// Checks whether the command is allowed in the supplied game state.
    /// </summary>
    /// <param name="command">
    /// The command to validate or execute.
    /// </param>
    /// <param name="gameState">
    /// The game state used by the operation.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when the command passes validation; otherwise, <see langword="false"/>.
    /// </returns>
    public bool Check(ICommand command, IReadOnlyGameState gameState);
}
