using Centuriin.CardGame.Core.Engine.Commands;
using Centuriin.CardGame.Core.Engine.Commands.Rules;

namespace Centuriin.CardGame.Core.Engine.Loading.Configuration.Commands;

/// <summary>
/// Builds command-specific and general validation rules.
/// </summary>
public interface IConfigurableCommandValidator
{
    /// <summary>
    /// Adds a rule to the validation of every registered command type.
    /// </summary>
    /// <typeparam name="TRule">
    /// The command validation rule type.
    /// </typeparam>
    /// <returns>
    /// This configuration builder.
    /// </returns>
    public IConfigurableCommandValidator AddGeneralRule<TRule>()
        where TRule : ICommandRule<ICommand>;

    /// <summary>
    /// Registers the ordered validation rules for a command type.
    /// </summary>
    /// <typeparam name="TCommand">
    /// The concrete command type.
    /// </typeparam>
    /// <returns>
    /// The validation configuration for the command type.
    /// </returns>
    public IConfigurableCommandValidation<TCommand> AddValidation<TCommand>()
        where TCommand : ICommand;
}
