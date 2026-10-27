using System.Collections.Immutable;

using Centuriin.CardGame.Core.Common.Commands.Rules;

namespace Centuriin.CardGame.Core.Common.Commands;

public interface ICommandValidator
{
    public bool IsValid(ICommand command);

    public void AddValidation(Type commandType, ImmutableList<ICommandRule> rules);
}
