using Centuriin.CardGame.Core.Common.Commands;
using Centuriin.CardGame.Core.Common.Commands.Rules;

namespace Centuriin.CardGame.Core.Common.Configuration.Commads;

public interface IConfigurableCommandValidations
{
    public IConfigurableCommandValidation<TCommand> AddValidation<TCommand>()
        where TCommand : ICommand;

    public IConfigurableCommandValidations AddGeneralRule<TRule>()
        where TRule : ICommandRule<ICommand>;
}
