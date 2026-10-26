using Centuriin.CardGame.Core.Common.Commands;

namespace Centuriin.CardGame.Core.Common.World;

public interface IGameSession : IDisposable
{
    public IGame Game { get; }

    public ICommandHandler CommandHandler { get; }
}
