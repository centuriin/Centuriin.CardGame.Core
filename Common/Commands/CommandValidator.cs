using System.Collections.Immutable;

using Centuriin.CardGame.Core.Common.Commands.Rules;
using Centuriin.CardGame.Core.Common.Observability.Logging;
using Centuriin.CardGame.Core.Common.Root.State;

namespace Centuriin.CardGame.Core.Common.Commands;

public sealed class CommandValidator : ICommandValidator
{
    private readonly ICoreLogger<CommandValidator> _logger;
    private readonly IGameState _gameState;
    private readonly Dictionary<Type, ImmutableList<ICommandRule>> _validationMap = [];

    public CommandValidator(
        ICoreLogger<CommandValidator> logger,
        IGameState gameState)
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
                $"Can not validate command {command.GetType().Name}, no rules exists.");
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
