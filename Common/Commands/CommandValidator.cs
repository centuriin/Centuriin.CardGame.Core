using System.Collections.Immutable;

using Centuriin.CardGame.Core.Common.Commands.Rules;
using Centuriin.CardGame.Core.Common.Events;
using Centuriin.CardGame.Core.Common.Observability.Logging;
using Centuriin.CardGame.Core.Common.World;

namespace Centuriin.CardGame.Core.Common.Commands;

public sealed class CommandValidator : ICommandValidator, IConfigurableCommandValidator
{
    private readonly ICoreLogger<CommandValidator> _logger;

    private IReadOnlyDictionary<Type, List<IGameRule>> RulesMap { get; set; } =
        ImmutableDictionary<Type, List<IGameRule>>.Empty;

    private IReadOnlyDictionary<Type, Func<ICommand, IPrimaryEvent>> FactoriesMap { get; set; } =
        ImmutableDictionary<Type, Func<ICommand, IPrimaryEvent>>.Empty;

    public CommandValidator(ICoreLogger<CommandValidator> logger)
    {
        ArgumentNullException.ThrowIfNull(logger);
        _logger = logger;
    }

    public void Configure(
        IReadOnlyDictionary<Type, List<IGameRule>> rulesMap, 
        IReadOnlyDictionary<Type, Func<ICommand, IPrimaryEvent>> factoriesMap)
    {
        ArgumentNullException.ThrowIfNull(rulesMap);
        RulesMap = rulesMap;

        ArgumentNullException.ThrowIfNull(factoriesMap);
        FactoriesMap = factoriesMap;
    }

    public IPrimaryEvent? Validate(IGameState gameState, ICommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        var commandType = command.GetType();

        var rules = RulesMap
            .Where(x => x.Key.IsAssignableFrom(commandType))
            .SelectMany(x => x.Value);

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

        if (_logger.IsEnabled(LogLevel.Debug))
        {
            _logger.LogDebug(
                "Command {CommadType} successfull validated",
                command.GetType().Name);
        }

        return FactoriesMap[commandType].Invoke(command);
    }
}