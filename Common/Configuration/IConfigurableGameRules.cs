using Centuriin.CardGame.Core.Common.Commands;
using Centuriin.CardGame.Core.Common.Commands.Rules;
using Centuriin.CardGame.Core.Common.Events.Domain;

namespace Centuriin.CardGame.Core.Common.Configuration;

public interface IConfigurableGameRules<TCommand>
    where TCommand : ICommand
{
    public IConfigurableGameRules<TCommand> AddRule<TGameRule>()
        where TGameRule : IGameRule;

    public IConfigurableCommandValidatation WithFactory<TEvent>(Func<TCommand, TEvent> factory)
        where TEvent : IGameEvent;
}