using Centuriin.CardGame.Core.Common.Configuration;
using Centuriin.CardGame.Core.Common.Configuration.Pipeline;

namespace Centuriin.CardGame.Core.Common.GameProfiles;

public interface IGameProfile
{
    public string Key { get; }

    public void Configure(IConfigurableGamePipeline configurablePipeLine);

    public void Configure(IConfigurableCommandValidatation configurableCommandValidatation);
}