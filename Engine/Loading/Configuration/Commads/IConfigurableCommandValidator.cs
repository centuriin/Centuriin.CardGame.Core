using Centuriin.CardGame.Core.Engine.Commands;
using Centuriin.CardGame.Core.Engine.Commands.Rules;

namespace Centuriin.CardGame.Core.Engine.Loading.Configuration.Commads;

public interface IConfigurableCommandValidator
{
    public IConfigurableCommandValidator AddGeneralRule<TRule>()
        where TRule : ICommandRule<ICommand>;

    public IConfigurableCommandValidation<TCommand> AddValidation<TCommand>()
        where TCommand : ICommand;
}
