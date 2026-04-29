using Centuriin.CardGame.Core.Common.Commands;
using Centuriin.CardGame.Core.Common.Commands.Rules;
using Centuriin.CardGame.Core.Common.Configuration;
using Centuriin.CardGame.Core.Common.Events;
using Centuriin.CardGame.Core.Common.GameProfiles;

namespace Centuriin.CardGame.Core.Console;

public sealed class MyTestProfile : IGameProfile
{
    public string Key => "TEST";

    public void Configure(IConfigurableGamePipeline configurablePipeLine)
    {
        ArgumentNullException.ThrowIfNull(configurablePipeLine);

        _ = configurablePipeLine
            .UseDefaultProfile();
    }

    public void Configure(IConfigurableCommandValidatation configurableCommandValidatation)
    {
        ArgumentNullException.ThrowIfNull(configurableCommandValidatation);

        _ = configurableCommandValidatation
            .AddValidation<ICommand>()
                .AddRule<ActorIsActivePlayerRule>();

        _ = configurableCommandValidatation
            .AddValidation<FakeCommand>()
                .AddRule<ActorIsActivePlayerRule>()
                .WithFactory(x => new GameStartedEvent(x.GameId));
    }
}
