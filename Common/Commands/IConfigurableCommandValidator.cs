using Centuriin.CardGame.Core.Common.Commands.Rules;
using Centuriin.CardGame.Core.Common.Events.Domain;

namespace Centuriin.CardGame.Core.Common.Commands;

public interface IConfigurableCommandValidator
{
    public void Configure(
        IReadOnlyDictionary<Type, List<IGameRule>> rulesMap, 
        IReadOnlyDictionary<Type, Func<ICommand, IGameEvent>> factoriesMap);
}