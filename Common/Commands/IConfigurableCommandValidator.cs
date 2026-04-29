using Centuriin.CardGame.Core.Common.Commands.Rules;
using Centuriin.CardGame.Core.Common.Events;

namespace Centuriin.CardGame.Core.Common.Commands;

public interface IConfigurableCommandValidator
{
    public void Configure(
        IReadOnlyDictionary<Type, List<IGameRule>> rulesMap, 
        IReadOnlyDictionary<Type, Func<ICommand, IPrimaryEvent>> factoriesMap);
}