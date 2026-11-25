using System.Collections.Immutable;

using Centuriin.CardGame.Core.Engine.Commands.Rules;

namespace Centuriin.CardGame.Core.Engine.Commands;

/// <summary>
/// Registers and evaluates validation rules for command types.
/// </summary>
public interface ICommandValidator
{
    /// <summary>
    /// Checks the command against its registered rules and stops at the first failed rule.
    /// </summary>
    /// <param name="command">
    /// The command to validate or execute.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when the command passes validation; otherwise, <see langword="false"/>.
    /// </returns>
    public bool IsValid(ICommand command);

    /// <summary>
    /// Registers the ordered validation rules for a command type.
    /// </summary>
    /// <param name="commandType">
    /// The concrete command type to register.
    /// </param>
    /// <param name="rules">
    /// The validation rules in evaluation order.
    /// </param>
    public void AddValidation(Type commandType, ImmutableList<ICommandRule> rules);
}
