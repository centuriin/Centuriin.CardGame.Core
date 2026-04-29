using Centuriin.CardGame.Core.Common.Commands;
using Centuriin.CardGame.Core.Common.Events;

namespace Centuriin.CardGame.Core.Common.Configuration;

public interface IConfigurableGameRules<TCommand>
    where TCommand : ICommand
{
    public IConfigurableGameRules<TCommand> AddRule<TGameRule>()
        where TGameRule : IGameRule;

    public IConfigurableCommandValidatationConfigurator WithFactory<TEvent>(Func<TCommand, TEvent> factory)
        where TEvent : IPrimaryEvent;
}