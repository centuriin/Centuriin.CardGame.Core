using Centuriin.CardGame.Core.Common.Events;
using Centuriin.CardGame.Core.Common.World;

namespace Centuriin.CardGame.Core.Common.Commands;

public interface ICommandValidator
{
    public IPrimaryEvent? Validate(IGameState state, ICommand command);
}