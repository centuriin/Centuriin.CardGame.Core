namespace Centuriin.CardGame.Core.Engine.Commands.Rules;

/// <summary>
/// Creates command validation rules.
/// </summary>
public interface ICommandRuleFactory
{
    /// <summary>
    /// Creates an instance of a command validation rule.
    /// </summary>
    /// <typeparam name="TRule">
    /// The command validation rule type.
    /// </typeparam>
    /// <returns>
    /// The created instance.
    /// </returns>
    public TRule Create<TRule>() where TRule : ICommandRule;
}