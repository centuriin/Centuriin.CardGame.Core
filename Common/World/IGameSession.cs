namespace Centuriin.CardGame.Core.Common.World;

public interface IGameSession : IDisposable
{
    public IGame Game { get; }
}
