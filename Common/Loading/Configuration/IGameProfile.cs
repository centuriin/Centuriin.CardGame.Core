using Centuriin.CardGame.Core.Common.Loading.Configuration.Commads;
using Centuriin.CardGame.Core.Common.Loading.Configuration.Pipeline;

namespace Centuriin.CardGame.Core.Common.Loading.Configuration;

public interface IGameProfile
{
    public string Key { get; }

    public void Configure(IConfigurableGamePipeline configurableGamePipeline);

    public void Configure(IConfigurableCommandValidator configurableCommandValidations);
}
