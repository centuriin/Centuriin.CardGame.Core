using System.Collections.Immutable;

using Centuriin.CardGame.Core.Engine.Commands;
using Centuriin.CardGame.Core.Engine.Commands.Rules;

namespace Centuriin.CardGame.Core.Engine.Loading.Configuration.Commands;

/// <summary>
/// Builds per-command validation rules and appends general rules when frozen.
/// </summary>
internal sealed class ConfigurableCommandValidator : IConfigurableCommandValidator
{
    private bool _isFrozen;

    private readonly Dictionary<Type, IConfigurableCommandValidation> _validations = [];

    private readonly Dictionary<Type, Func<ICommandRuleFactory, ICommandRule>> _generalRules = [];

    /// <summary>
    /// Prevents further configuration and combines command-specific and general rule factories.
    /// </summary>
    /// <returns>
    /// The completed configurations in their configured order.
    /// </returns>
    public IReadOnlyDictionary<Type, ImmutableList<Func<ICommandRuleFactory, ICommandRule>>> Freeze()
    {
        _isFrozen = true;

        return _validations.ToDictionary(
            k => k.Key,
            v => v.Value.Rules.Concat(_generalRules.Values).ToImmutableList());
    }

    /// <inheritdoc/>
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

    /// <inheritdoc/>
    public IConfigurableCommandValidation<TCommand> AddValidation<TCommand>()
        where TCommand : ICommand
    {
        ThrowIfFrozen();

        var configurable = new ConfigurableCommandValidation<TCommand>()
        {
            Configurable = this
        };

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
                "Cannot change a frozen configuration.");
        }
    }

    /// <summary>
    /// Configures validation rules for a particular command type.
    /// </summary>
    private interface IConfigurableCommandValidation
    {
        /// <summary>
        /// Gets the factories used to create this command's validation rules.
        /// </summary>
        public List<Func<ICommandRuleFactory, ICommandRule>> Rules { get; }
    }

    /// <summary>
    /// Collects the rule factories associated with one command type.
    /// </summary>
    /// <typeparam name="TCommand">
    /// The concrete command type.
    /// </typeparam>
    private sealed class ConfigurableCommandValidation<TCommand> :
        IConfigurableCommandValidation<TCommand>,
        IConfigurableCommandValidation
        where TCommand : ICommand
    {
        private readonly HashSet<Type> _ruleTypes = [];

        private readonly List<Func<ICommandRuleFactory, ICommandRule>> _rules = [];

        /// <summary>
        /// Gets the owner of this command validation configuration.
        /// </summary>
        public required ConfigurableCommandValidator Configurable { get; init; }

        /// <inheritdoc/>
        public IConfigurableCommandValidator CommandValidator => Configurable;

        /// <inheritdoc/>
        public List<Func<ICommandRuleFactory, ICommandRule>> Rules => _rules;

        /// <inheritdoc/>
        public IConfigurableCommandValidation<TCommand> AddRule<TRule>()
            where TRule : ICommandRule<TCommand>
        {
            Configurable.ThrowIfFrozen();

            if (!_ruleTypes.Add(typeof(TRule)))
            {

                throw new InvalidOperationException(
                    $"Rule {typeof(TRule).Name} for command" +
                    $" {typeof(TCommand).Name} already added.");
            }
            _rules.Add(factory => factory.Create<TRule>());

            return this;
        }
    }
}
