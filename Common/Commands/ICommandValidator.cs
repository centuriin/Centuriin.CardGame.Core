using Centuriin.CardGame.Core.Common.Events.Domain;
using Centuriin.CardGame.Core.Common.World;

namespace Centuriin.CardGame.Core.Common.Commands;

public interface ICommandValidator
{
    public IGameEvent? Validate(IGameState state, ICommand command);
}