using Centuriin.CardGame.Core.Engine.Loading.Configuration;

namespace Centuriin.CardGame.Core.Application.Modulation;

public interface IGameModule
{
    public string Key { get; }

    public IGameProfile GameProfile { get; }
}