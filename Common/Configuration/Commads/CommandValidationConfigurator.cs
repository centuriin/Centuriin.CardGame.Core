using Centuriin.CardGame.Core.Common.Commands;
using Centuriin.CardGame.Core.Common.Commands.Rules;

namespace Centuriin.CardGame.Core.Common.Configuration.Commads;

public sealed class CommandValidationConfigurator : ConfiguratorBase, IConfigurableCommandValidator
{
    private readonly ICommandRuleFactory _ruleFactory;
    private readonly ICommandValidator _validator;

    private readonly Dictionary<Type, IConfigurableCommandValidation> _validationMap = [];

    private readonly Dictionary<Type, ICommandRule> _generalRules = [];

    public CommandValidationConfigurator(
        ICommandRuleFactory ruleFactory,
        ICommandValidator validator)
    {
        ArgumentNullException.ThrowIfNull(ruleFactory);
        _ruleFactory = ruleFactory;

        ArgumentNullException.ThrowIfNull(validator);
        _validator = validator;
    }

    /// <inheritdoc/>
    public IConfigurableCommandValidation<TCommand> AddValidation<TCommand>()
        where TCommand : ICommand
    {
        ThrowIfConfigured();

        var commandType = typeof(TCommand);

        if (_validationMap.TryGetValue(commandType, out var _))
        {
            throw new InvalidOperationException(
                $"Command {commandType.Name} already added.");
        }

        var configurable = new ConfigurableCommandValidation<TCommand>()
        {
            Configurator = this
        };

        _validationMap[commandType] = configurable;

        return configurable;
    }

    /// <inheritdoc/>
    public IConfigurableCommandValidator AddGeneralRule<TRule>()
        where TRule : ICommandRule<ICommand>
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
    protected override void ConfigureCore()
    {
        var validations = new Dictionary<Type, IEnumerable<ICommandRule>>();

        foreach (var (commandType, configurable) in _validationMap)
        {
            validations[commandType] = [.. configurable.Rules, .. _generalRules.Values];
        }

        _validator.Setup(validations);
    }

    private sealed class ConfigurableCommandValidation<TCommand> :
        IConfigurableCommandValidation<TCommand>,
        IConfigurableCommandValidation
        where TCommand : ICommand
    {
        public required CommandValidationConfigurator Configurator { get; init; }

        public IConfigurableCommandValidator CommandValidator => Configurator;

        public IEnumerable<ICommandRule> Rules => RulesMap.Values;

        public Dictionary<Type, ICommandRule<TCommand>> RulesMap { get; } = [];

        public IConfigurableCommandValidation<TCommand> AddRule<TRule>()
            where TRule : ICommandRule<TCommand>
        {
            var ruleType = typeof(TRule);

            if (RulesMap.TryGetValue(ruleType, out var _))
            {
                throw new InvalidOperationException(
                    $"Rule {ruleType.Name} for command {typeof(TCommand).Name}" +
                    $" already added.");
            }

            RulesMap[ruleType] = Configurator._ruleFactory.Create<TRule>();

            return this;
        }
    }

    private interface IConfigurableCommandValidation
    {
        public IEnumerable<ICommandRule> Rules { get; }
    }
}
