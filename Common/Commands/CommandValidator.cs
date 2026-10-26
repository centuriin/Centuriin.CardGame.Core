using Centuriin.CardGame.Core.Common.Commands.Rules;
using Centuriin.CardGame.Core.Common.Observability.Logging;
using Centuriin.CardGame.Core.Common.World;

namespace Centuriin.CardGame.Core.Common.Commands;

public sealed class CommandValidator : ICommandValidator
{
    private readonly ICoreLogger<CommandValidator> _logger;
    private readonly IGameState _gameState;
    private IReadOnlyDictionary<Type, IEnumerable<ICommandRule>>? _commandValidationMap;


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
    public bool IsValid<TCommand>(TCommand command)
        where TCommand : ICommand
    {
        ArgumentNullException.ThrowIfNull(command);

        if (_commandValidationMap is null)
        {
            throw new InvalidOperationException("Validations must be added.");
        }

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

        foreach (var rule in rules.Cast<ICommandRule<TCommand>>())
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

    /// <inheritdoc/>
    void ICommandValidator.Setup(
        IReadOnlyDictionary<Type, IEnumerable<ICommandRule>> validations)
    {
        ArgumentNullException.ThrowIfNull(validations);

        if (_commandValidationMap is not null)
        {
            throw new InvalidOperationException("Validations already added.");
        }

        _commandValidationMap = validations;
    }
}