using Centuriin.CardGame.Core.Common.Commands.Rules;
using Centuriin.CardGame.Core.Common.Observability.Logging;
using Centuriin.CardGame.Core.Common.World;

namespace Centuriin.CardGame.Core.Common.Commands;

public sealed class CommandValidator : ICommandValidator
{
    private readonly ICoreLogger<CommandValidator> _logger;
    private readonly IReadOnlyDictionary<Type, IEnumerable<IGameRule>> _commandValidationMap;
    private readonly IGameState _gameState;

    public CommandValidator(
        ICoreLogger<CommandValidator> logger,
        IReadOnlyDictionary<Type, IEnumerable<IGameRule>> commandValidationMap,
        IGameState gameState)
    {
        ArgumentNullException.ThrowIfNull(logger);
        _logger = logger;

        ArgumentNullException.ThrowIfNull(commandValidationMap);
        _commandValidationMap = commandValidationMap;

        ArgumentNullException.ThrowIfNull(gameState);
        _gameState = gameState;
    }

    /// <inheritdoc/>
    public bool IsValid(ICommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        if (!_commandValidationMap.TryGetValue(command.GetType(), out var rules))
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
            if (rule.Check(command.Actor, _gameState))
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