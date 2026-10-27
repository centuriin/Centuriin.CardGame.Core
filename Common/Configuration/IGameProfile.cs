using Centuriin.CardGame.Core.Common.Configuration.Commads;
using Centuriin.CardGame.Core.Common.Configuration.Pipeline;

namespace Centuriin.CardGame.Core.Common.Configuration;

public interface IGameProfile
{
    public string Key { get; }

    public void Configure(IConfigurableGamePipeline configurableGamePipeline);

    public void Configure(IConfigurableCommandValidator configurableCommandValidations);
}
