using Centuriin.CardGame.Core.Common.World;

namespace Centuriin.CardGame.Core.Common.Commands;

public interface ICommandExecutorFactory
{
    public ICommandExecutor Create(IGame game);
}