using Centuriin.CardGame.Core.Common.Commands;
using Centuriin.CardGame.Core.Common.Commands.Rules;

namespace Centuriin.CardGame.Core.Common.Configuration.Commads;

public interface IConfigurableCommandValidator
{
    public IConfigurableCommandValidator AddGeneralRule<TRule>()
        where TRule : ICommandRule<ICommand>;

    public IConfigurableCommandValidation<TCommand> AddValidation<TCommand>()
        where TCommand : ICommand;
}
