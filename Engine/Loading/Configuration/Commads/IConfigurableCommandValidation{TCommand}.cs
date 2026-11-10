using Centuriin.CardGame.Core.Engine.Commands;
using Centuriin.CardGame.Core.Engine.Commands.Rules;

namespace Centuriin.CardGame.Core.Engine.Loading.Configuration.Commads;

public interface IConfigurableCommandValidation<TCommand>
    where TCommand : ICommand
{
    public IConfigurableCommandValidator CommandValidator { get; }

    public IConfigurableCommandValidation<TCommand> AddRule<TRule>()
        where TRule : ICommandRule<TCommand>;
}
