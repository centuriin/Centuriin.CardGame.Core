using Centuriin.CardGame.Core.Common.Commands;
using Centuriin.CardGame.Core.Common.Events;

namespace Centuriin.CardGame.Core.Common.Configuration;

public interface IConfigurableCommandValidatation
{
    public IConfigurableGameRules<TCommand> AddValidation<TCommand>()
        where TCommand : ICommand;
}