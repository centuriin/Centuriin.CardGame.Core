using Centuriin.CardGame.Core.Common.Commands;
using Centuriin.CardGame.Core.Common.Events;
using Centuriin.CardGame.Core.Common.Factories;

namespace Centuriin.CardGame.Core.Common.Configuration;

public sealed class CommandValidatationConfigurator :
    ConfiguratorBase,
    ICommandValidatationConfigurator
{
    private readonly Dictionary<Type, object> _gameRulesConfigurators = [];

    private readonly IConfigurableCommandValidator _configurableValidator;

    private Dictionary<Type, List<IGameRule>> GameRules { get; } = [];

    private Dictionary<Type, Func<ICommand, IPrimaryEvent>> EventFactories { get; } = [];

    private IRuleFactory RuleFactory { get; }

    public CommandValidatationConfigurator(
        IRuleFactory ruleFactory,
        IConfigurableCommandValidator configurableValidator)
    {
        ArgumentNullException.ThrowIfNull(ruleFactory);
        RuleFactory = ruleFactory;

        ArgumentNullException.ThrowIfNull(configurableValidator);
        _configurableValidator = configurableValidator;
    }

    public IConfigurableGameRules<TCommand> AddValidation<TCommand>()
        where TCommand : ICommand
    {
        ThrowIfInitialized();

        var commandType = typeof(TCommand);

        if (!_gameRulesConfigurators.TryGetValue(commandType, out var configurable))
        {
            configurable = new ConfigurableGameRules<TCommand>(this);

            _gameRulesConfigurators[commandType] = configurable;
        }

        return (IConfigurableGameRules<TCommand>)configurable;
    }

    protected override void SetupCore() => _configurableValidator.Configure(GameRules, EventFactories);

    private sealed class ConfigurableGameRules<TCommand> : IConfigurableGameRules<TCommand>
        where TCommand : ICommand
    {
        private readonly CommandValidatationConfigurator _configurator;

        private static Type CommandType { get; } = typeof(TCommand);

        public ConfigurableGameRules(CommandValidatationConfigurator configurator)
        {
            _configurator = configurator;
        }

        public IConfigurableGameRules<TCommand> AddRule<TGameRule>() where TGameRule : IGameRule
        {
            _configurator.ThrowIfInitialized();

            if (!_configurator.GameRules.TryGetValue(CommandType, out var rules))
            {
                rules = [];
                _configurator.GameRules[CommandType] = rules;
            }

            rules.Add(_configurator.RuleFactory.Create<TGameRule>());

            return this;
        }

        public IConfigurableCommandValidatationConfigurator WithFactory<TEvent>(Func<TCommand, TEvent> factory)
            where TEvent : IPrimaryEvent
        {
            _configurator.ThrowIfInitialized();

            if (_configurator.EventFactories.ContainsKey(CommandType))
            {
                throw new InvalidOperationException();
            }

            _configurator.EventFactories[CommandType] = (command) => factory.Invoke((TCommand)command);

            return _configurator;
        }
    }
}