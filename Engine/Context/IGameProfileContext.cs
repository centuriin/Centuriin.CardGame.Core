using Centuriin.CardGame.Core.Engine.Loading.Configuration;

namespace Centuriin.CardGame.Core.Engine.Context;

public interface IGameProfileContext : IGameContext
{
    public IGameProfile Profile { get; }
}