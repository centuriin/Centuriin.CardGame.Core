using Centuriin.CardGame.Core.Common.Commands.Rules;

namespace Centuriin.CardGame.Core.Common.Commands;

public interface ICommandValidator
{
    public bool IsValid<TCommand>(TCommand command)
        where TCommand : ICommand;

    internal void Setup(IReadOnlyDictionary<Type, IEnumerable<ICommandRule>> validations);
}