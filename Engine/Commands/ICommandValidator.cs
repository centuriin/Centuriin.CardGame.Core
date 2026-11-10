using System.Collections.Immutable;

using Centuriin.CardGame.Core.Engine.Commands.Rules;

namespace Centuriin.CardGame.Core.Engine.Commands;

public interface ICommandValidator
{
    public bool IsValid(ICommand command);

    public void AddValidation(Type commandType, ImmutableList<ICommandRule> rules);
}
