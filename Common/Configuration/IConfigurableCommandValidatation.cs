using Centuriin.CardGame.Core.Common.Commands;
using Centuriin.CardGame.Core.Common.Commands.Rules;

namespace Centuriin.CardGame.Core.Common.Configuration;

public interface IConfigurableCommandValidatation
{
    public IConfigurableCommandValidatation UseDefaultProfile();

    public IConfigurableGameRules<TCommand> AddValidation<TCommand>()
        where TCommand : ICommand;

    public IConfigurableCommandValidatation AddGeneralRule<TGameRule>()
        where TGameRule : IGameRule;
}