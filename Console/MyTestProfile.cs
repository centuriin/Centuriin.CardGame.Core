using Centuriin.CardGame.Core.Common.Commands.Rules;
using Centuriin.CardGame.Core.Common.Configuration.Commads;
using Centuriin.CardGame.Core.Common.Configuration.Pipeline;
using Centuriin.CardGame.Core.Common.GameProfiles;

namespace Centuriin.CardGame.Core.Console;

public sealed class MyTestProfile : IGameProfile
{
    public string Key => "TEST";

    public void Configure(IConfigurableGamePipeline configurableGamePipeLine)
    {
        ArgumentNullException.ThrowIfNull(configurableGamePipeLine);

        _ = configurableGamePipeLine
            .UseDefaultProfile();
    }

    public void Configure(IConfigurableCommandValidator configurableGamePipeLine)
    {
        ArgumentNullException.ThrowIfNull(configurableGamePipeLine);

        _ = configurableGamePipeLine
            .AddGeneralRule<ActorIsActivePlayerRule>()
            .AddValidation<FakeCommand>()
                .AddRule<ActorIsActivePlayerRule>();
    }
}
