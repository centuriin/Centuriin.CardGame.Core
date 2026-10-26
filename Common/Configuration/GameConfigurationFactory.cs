using Centuriin.CardGame.Core.Common.Configuration.Commads;
using Centuriin.CardGame.Core.Common.Configuration.Pipeline;
using Centuriin.CardGame.Core.Common.GameProfiles;

namespace Centuriin.CardGame.Core.Common.Configuration;

public sealed class GameConfigurationFactory : IGameConfigurationFactory
{
    public IGameConfiguration CreateFromProfile(IGameProfile profile)
    {
        ArgumentNullException.ThrowIfNull(profile);

        var configurableGamePipeline = new ConfigurableGamePipeline();
        profile.Configure(configurableGamePipeline);
        configurableGamePipeline.Freeze();

        var configurableCommandValidations = new ConfigurableCommandValidations();
        profile.Configure(configurableCommandValidations);
        configurableCommandValidations.Freeze();

        return new GameConfiguration(
            configurableGamePipeline, 
            configurableCommandValidations);
    }
}
