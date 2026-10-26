using Centuriin.CardGame.Core.Common.GameProfiles;

namespace Centuriin.CardGame.Core.Common.Configuration;

public interface IGameConfigurationFactory
{
    public IGameConfiguration CreateFromProfile(IGameProfile profile);
}