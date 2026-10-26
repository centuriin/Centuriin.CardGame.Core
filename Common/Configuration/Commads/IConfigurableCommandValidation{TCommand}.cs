using Centuriin.CardGame.Core.Common.Commands;
using Centuriin.CardGame.Core.Common.Commands.Rules;

namespace Centuriin.CardGame.Core.Common.Configuration.Commads;

public interface IConfigurableCommandValidation<TCommand>
    where TCommand : ICommand
{
    public IConfigurableCommandValidations CommandValidator { get; }

    public IConfigurableCommandValidation<TCommand> AddRule<TRule>()
        where TRule : ICommandRule<TCommand>;
}
