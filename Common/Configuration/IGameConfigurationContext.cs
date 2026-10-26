using Centuriin.CardGame.Core.Common.Commands;
using Centuriin.CardGame.Core.Common.Commands.Rules;
using Centuriin.CardGame.Core.Common.ECS.Systems;
using Centuriin.CardGame.Core.Common.Events.Dispatching;
using Centuriin.CardGame.Core.Common.Events.Domain;

namespace Centuriin.CardGame.Core.Common.Configuration;

public interface IGameConfigurationContext
{
    public ISystemFactory SystemFactory { get; }

    public IDispatcher<IGameEvent> Dispatcher { get; }

    public ICommandRuleFactory RuleFactory { get; }

    public ICommandValidator CommandValidator { get; }
}
