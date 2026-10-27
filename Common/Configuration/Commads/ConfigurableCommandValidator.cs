using System.Collections.Immutable;

using Centuriin.CardGame.Core.Common.Commands;
using Centuriin.CardGame.Core.Common.Commands.Rules;

namespace Centuriin.CardGame.Core.Common.Configuration.Commads;

internal sealed class ConfigurableCommandValidator : IConfigurableCommandValidator
{
    private bool _isFrozen;

    private readonly Dictionary<Type, IConfigurableCommandValidation> _validations = [];
    
    private readonly Dictionary<Type, Func<ICommandRuleFactory, ICommandRule>> _generalRules = [];

    public IReadOnlyDictionary<Type, ImmutableList<Func<ICommandRuleFactory, ICommandRule>>> Freeze()
    {
        _isFrozen = true;

        return _validations.ToDictionary(
            k => k.Key,
            v => v.Value.Rules.Concat(_generalRules.Values).ToImmutableList());
    }

    public IConfigurableCommandValidator AddGeneralRule<TRule>() where TRule : ICommandRule<ICommand>
    {
        ThrowIfFrozen();

        if (!_generalRules.TryAdd(typeof(TRule), factory => factory.Create<TRule>()))
        {
            throw new InvalidOperationException(
                $"General rule {typeof(TRule).Name} already added.");
        }

        return this;
    }

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
        public List<Func<ICommandRuleFactory, ICommandRule>> Rules { get; }
    }

    private sealed class ConfigurableCommandValidation<TCommand>(
        ConfigurableCommandValidator configurator,
        List<Func<ICommandRuleFactory, ICommandRule>> rules) : 
        IConfigurableCommandValidation<TCommand>,
        IConfigurableCommandValidation
        where TCommand : ICommand
    {
        private readonly HashSet<Type> _ruleTypes = [];

        public IConfigurableCommandValidator CommandValidator => configurator;

        public List<Func<ICommandRuleFactory, ICommandRule>> Rules { get; } = [];

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
