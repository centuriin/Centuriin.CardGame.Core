using Centuriin.CardGame.Core.Common.Commands;
using Centuriin.CardGame.Core.Common.Commands.Rules;

namespace Centuriin.CardGame.Core.Common.Configuration.Commads;

public sealed class CommandValidationConfigurator : ConfiguratorBase, IConfigurableCommandValidator
{
    private readonly IRuleFactory _ruleFactory;

    private readonly Dictionary<Type, ConfigurableCommandValidation> _validationMap = [];

    private readonly Dictionary<Type, IGameRule> _generalRules = [];

    public CommandValidationConfigurator(IRuleFactory ruleFactory)
    {
        ArgumentNullException.ThrowIfNull(ruleFactory);
        _ruleFactory = ruleFactory;
    }

    /// <inheritdoc/>
    public IConfigurableCommandValidation AddValidation<TCommand>()
        where TCommand : ICommand
    {
        ThrowIfConfigured();

        var commandType = typeof(TCommand);

        if (_validationMap.TryGetValue(commandType, out var _))
        {
            throw new InvalidOperationException(
                $"Command {commandType.Name} already added.");
        }

        var configurable = new ConfigurableCommandValidation()
        {
            CommandType = commandType,
            Configurator = this
        };

        _validationMap[commandType] = configurable;

        return configurable;
    }

    /// <inheritdoc/>
    public IConfigurableCommandValidator AddGeneralRule<TRule>()
        where TRule : IGameRule
    {
        var ruleType = typeof(TRule);

        if (_generalRules.TryGetValue(ruleType, out var _))
        {
            throw new InvalidOperationException(
                $"General rule {ruleType.Name} already added.");
        }

        _generalRules[ruleType] = _ruleFactory.Create<TRule>();

        return this;
    }

    /// <inheritdoc/>
    protected override void ConfigureCore() => throw new NotImplementedException();

    private sealed class ConfigurableCommandValidation : IConfigurableCommandValidation
    {
        private Dictionary<Type, IGameRule> Rules { get; } = [];

        public required Type CommandType { get; init; }

        public required CommandValidationConfigurator Configurator { get; init; }

        public IConfigurableCommandValidator CommandValidator => Configurator;

        public IConfigurableCommandValidation AddRule<TRule>()
            where TRule : IGameRule
        {
            var ruleType = typeof(TRule);

            if (Rules.TryGetValue(ruleType, out var _))
            {
                throw new InvalidOperationException(
                    $"Rule {ruleType.Name} for command {CommandType.Name}" +
                    $" already added.");
            }

            Rules[ruleType] = Configurator._ruleFactory.Create<TRule>();

            return this;
        }
    }
}
