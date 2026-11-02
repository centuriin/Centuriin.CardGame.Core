using Centuriin.CardGame.Core.Common.Loading.Configuration;

namespace Centuriin.CardGame.Core.Common.Context;

public interface IGameProfileContext : IGameContext
{
    public IGameProfile Profile { get; }
}