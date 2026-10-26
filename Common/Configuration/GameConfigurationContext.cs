using Centuriin.CardGame.Core.Common.Commands;
using Centuriin.CardGame.Core.Common.Commands.Rules;
using Centuriin.CardGame.Core.Common.ECS.Systems;
using Centuriin.CardGame.Core.Common.Events.Dispatching;
using Centuriin.CardGame.Core.Common.Events.Domain;

namespace Centuriin.CardGame.Core.Common.Configuration;

public sealed class GameConfigurationContext : IGameConfigurationContext
{
    public ISystemFactory SystemFactory { get; }

    public IDispatcher<IGameEvent> Dispatcher { get; }

    public ICommandRuleFactory RuleFactory { get; }

    public ICommandValidator CommandValidator { get; }

    public GameConfigurationContext(
        ISystemFactory systemFactory,
        IDispatcher<IGameEvent> dispatcher,
        ICommandRuleFactory ruleFactory,
        ICommandValidator commandValidator)
    {
        ArgumentNullException.ThrowIfNull(systemFactory);
        SystemFactory = systemFactory;

        ArgumentNullException.ThrowIfNull(dispatcher);
        Dispatcher = dispatcher;

        ArgumentNullException.ThrowIfNull(ruleFactory);
        RuleFactory = ruleFactory;

        ArgumentNullException.ThrowIfNull(commandValidator);
        CommandValidator = commandValidator;
    }
}