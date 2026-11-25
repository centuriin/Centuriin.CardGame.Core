using Centuriin.CardGame.Core.Engine.Commands;
using Centuriin.CardGame.Core.Engine.Commands.Rules;

namespace Centuriin.CardGame.Core.Engine.Loading.Configuration.Commands;

/// <summary>
/// Configures validation rules for a particular command type.
/// </summary>
/// <typeparam name="TCommand">
/// The concrete command type.
/// </typeparam>
public interface IConfigurableCommandValidation<TCommand>
    where TCommand : ICommand
{
    /// <summary>
    /// Gets the parent validator configuration to continue configuring commands.
    /// </summary>
    public IConfigurableCommandValidator CommandValidator { get; }

    /// <summary>
    /// Adds a validation rule for the configured command type.
    /// </summary>
    /// <typeparam name="TRule">
    /// The command validation rule type.
    /// </typeparam>
    /// <returns>
    /// This command validation configuration.
    /// </returns>
    public IConfigurableCommandValidation<TCommand> AddRule<TRule>()
        where TRule : ICommandRule<TCommand>;
}
