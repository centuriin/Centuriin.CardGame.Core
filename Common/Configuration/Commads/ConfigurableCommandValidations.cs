using Centuriin.CardGame.Core.Common.Commands;
using Centuriin.CardGame.Core.Common.Commands.Rules;

namespace Centuriin.CardGame.Core.Common.Configuration.Commads;

internal sealed class ConfigurableCommandValidations : ICommandValidations
{
    private bool _isFrozen;

    private readonly Dictionary<Type, IConfigurableCommandValidation> _validations = [];
    
    private readonly Dictionary<Type, Func<ICommandRuleFactory, ICommandRule>> _generalRules = [];

    public IConfigurableCommandValidation<TCommand> AddValidation<TCommand>() 
        where TCommand : ICommand
    {
        ThrowIfFrozen();

        var configurable = new ConfigurableCommandValidation<TCommand>(this, []);

        if (!_validations.TryAdd(typeof(TCommand), configurable))
        {
            throw new InvalidOperationException(
                $"Command {typeof(TCommand).Name} already added.");
        }
        
        return configurable;
    }

    public IConfigurableCommandValidations AddGeneralRule<TRule>() where TRule : ICommandRule<ICommand>
    {
        ThrowIfFrozen();

        if (!_generalRules.TryAdd(typeof(TRule), factory => factory.Create<TRule>()))
        {
            throw new InvalidOperationException(
                $"General rule {typeof(TRule).Name} already added.");
        }
        
        return this;
    }

    public void Freeze() => _isFrozen = true;

    public void Apply(ICommandValidator validator, ICommandRuleFactory factory)
    {
        if (!_isFrozen)
        {
            throw new InvalidOperationException("Configuration must be frozen.");
        }

        var validations = new Dictionary<Type, IEnumerable<ICommandRule>>();

        foreach (var (commandType, configurable) in _validations)
        {
            var rules = configurable.Rules
                .Concat(_generalRules.Values)
                .Select(x => x.Invoke(factory))
                .ToArray();

            validations[commandType] = rules;
        }

        validator.UseValidations(validations);
    }

    private void ThrowIfFrozen()
    {
        if (_isFrozen)
        {
            throw new InvalidOperationException(
                "Can not change frozen configuration.");
        }
    }

    private interface IConfigurableCommandValidation
    {
        public IReadOnlyCollection<Func<ICommandRuleFactory, ICommandRule>> Rules { get; }
    }

    private sealed class ConfigurableCommandValidation<TCommand>(
        ConfigurableCommandValidations configurator,
        List<Func<ICommandRuleFactory, ICommandRule>> rules) : 
        IConfigurableCommandValidation<TCommand>,
        IConfigurableCommandValidation
        where TCommand : ICommand
    {
        private readonly HashSet<Type> _ruleTypes = [];

        public IConfigurableCommandValidations CommandValidator => configurator;

        public IReadOnlyCollection<Func<ICommandRuleFactory, ICommandRule>> Rules => rules;

        public IConfigurableCommandValidation<TCommand> AddRule<TRule>() 
            where TRule : ICommandRule<TCommand>
        {
            configurator.ThrowIfFrozen();

            if (!_ruleTypes.Add(typeof(TRule)))
            {

                throw new InvalidOperationException(
                    $"Rule {typeof(TRule).Name} for command" +
                    $" {typeof(TCommand).Name} already added.");
            }
            rules.Add(factory => factory.Create<TRule>());

            return this;
        }
    }
}
