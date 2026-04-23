using Centuriin.CardGame.Core.Common.Events;

namespace Centuriin.CardGame.Core.Common.Commands;

public interface ICommandValidator
{
    public IPrimaryEvent? Validate(ICommand command);
}