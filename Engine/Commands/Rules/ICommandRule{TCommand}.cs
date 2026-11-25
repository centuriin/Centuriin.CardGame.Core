using Centuriin.CardGame.Core.Engine.Root.State;

namespace Centuriin.CardGame.Core.Engine.Commands.Rules;

/// <summary>
/// Checks whether a command is allowed in the current game state.
/// </summary>
/// <typeparam name="TCommand">
/// The concrete command type.
/// </typeparam>
public interface ICommandRule<in TCommand> : ICommandRule
    where TCommand : ICommand
{
    /// <inheritdoc cref="ICommandRule.Check"/>
    public bool Check(TCommand command, IReadOnlyGameState gameState);

    /// <inheritdoc/>
    bool ICommandRule.Check(ICommand command, IReadOnlyGameState gameState) =>
        Check((TCommand)command, gameState);
}
