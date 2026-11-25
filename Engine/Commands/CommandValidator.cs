using System.Collections.Immutable;

using Centuriin.CardGame.Core.Engine.Commands.Rules;
using Centuriin.CardGame.Core.Engine.Observability.Logging;
using Centuriin.CardGame.Core.Engine.Root.State;

namespace Centuriin.CardGame.Core.Engine.Commands;

/// <summary>
/// Checks commands against their registered validation rules.
/// </summary>
public sealed class CommandValidator : ICommandValidator
{
    private readonly ICoreLogger<CommandValidator> _logger;
    private readonly IReadOnlyGameState _gameState;
    private readonly Dictionary<Type, ImmutableList<ICommandRule>> _validationMap = [];

    /// <summary>
    /// Initializes a new <see cref="CommandValidator"/> instance.
    /// </summary>
    /// <param name="logger">
    /// The logger used for diagnostics.
    /// </param>
    /// <param name="gameState">
    /// The game state used by the operation.
    /// </param>
    public CommandValidator(
        ICoreLogger<CommandValidator> logger,
        IReadOnlyGameState gameState)
    {
        ArgumentNullException.ThrowIfNull(logger);
        _logger = logger;

        ArgumentNullException.ThrowIfNull(gameState);
        _gameState = gameState;
    }

    /// <inheritdoc/>
    public void AddValidation(
        Type commandType,
        ImmutableList<ICommandRule> rules)
    {
        ArgumentNullException.ThrowIfNull(commandType);
        ArgumentNullException.ThrowIfNull(rules);

        if (_validationMap.TryGetValue(commandType, out var _))
        {
            throw new InvalidOperationException(
                $"Validation for {commandType.Name} already added.");
        }

        _validationMap[commandType] = rules;
    }

    /// <inheritdoc/>
    public bool IsValid(ICommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        if (!_validationMap.TryGetValue(command.GetType(), out var rules))
        {
            throw new InvalidOperationException(
                $"Cannot validate command {command.GetType().Name}, no rules are registered.");
        }

        if (_logger.IsEnabled(LogLevel.Debug))
        {
            _logger.LogDebug(
                "Start validation for command {Command}",
                command.GetType().Name);
        }

        foreach (var rule in rules)
        {
            if (rule.Check(command, _gameState))
            {
                if (_logger.IsEnabled(LogLevel.Debug))
                {
                    _logger.LogDebug(
                        "Rule {Rule} passed for command {Command}",
                        rule.GetType().Name,
                        command.GetType().Name);
                }
            }
            else
            {
                _logger.LogWarning(
                    "Rule {Rule} not passed for command {Command}",
                    rule.GetType().Name,
                    command.GetType().Name);

                return false;
            }
        }

        return true;
    }
}
