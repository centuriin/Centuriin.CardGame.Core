using Centuriin.CardGame.Core.Common.Commands;
using Centuriin.CardGame.Core.Common.Commands.Rules;

namespace Centuriin.CardGame.Core.Common.Configuration.Commads;

public interface IConfigurableCommandValidator
{
    public IConfigurableCommandValidation AddValidation<TCommand>()
        where TCommand : ICommand;

    public IConfigurableCommandValidator AddGeneralRule<TRule>()
        where TRule : IGameRule;
}
