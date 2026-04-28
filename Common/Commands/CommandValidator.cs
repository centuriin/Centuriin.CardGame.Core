using Centuriin.CardGame.Core.Common.Events;
using Centuriin.CardGame.Core.Common.Observability.Logging;
using Centuriin.CardGame.Core.Common.World;

namespace Centuriin.CardGame.Core.Common.Commands;

public sealed class CommandValidator : ICommandValidator, IConfigurableCommandValidator
{
    private readonly Dictionary<Type, List<IGameRule>> _rulesMap = [];

    private readonly ICoreLogger<CommandValidator> _logger;

    public CommandValidator(ICoreLogger<CommandValidator> logger)
    {
        ArgumentNullException.ThrowIfNull(logger);
        _logger = logger;
    }

    public void AddRule<TCommand>(IGameRule rule)
        where TCommand : ICommand
    {
        ArgumentNullException.ThrowIfNull(rule);

        var commandType = typeof(TCommand);

        if (!_rulesMap.TryGetValue(commandType, out var rules))
        {
            rules = [];
            _rulesMap[commandType] = rules;
        }

        rules.Add(rule);
    }

    public IPrimaryEvent? Validate(IGameState gameState, ICommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        var rules = _rulesMap[command.GetType()];

        foreach (var rule in rules)
        {
            var result = rule.Check(gameState, command);

            if (result is { ErrorMessage: not null })
            {
                if (_logger.IsEnabled(LogLevel.Debug))
                {
                    _logger.LogDebug(
                        "Rule {RuleName} declined command by reason: {RuleErrorMessage}",
                        command.GetType().Name,
                        result.ErrorMessage);
                }

                return null;
            }
        }

        return null;
    }
}
