using Centuriin.CardGame.Core.Engine.Loading.Configuration.Commads;
using Centuriin.CardGame.Core.Engine.Loading.Configuration.Pipeline;

namespace Centuriin.CardGame.Core.Engine.Loading.Configuration;

public interface IGameProfile
{
    public string Key { get; }

    public void Configure(IConfigurableGamePipeline configurableGamePipeline);

    public void Configure(IConfigurableCommandValidator configurableCommandValidations);
}
