using Centuriin.CardGame.Core.Common.World;

namespace Centuriin.CardGame.Core.Common.Commands;

public interface ICommandHandlerFactory
{
    public ICommandHandler Create(IGame game);
}