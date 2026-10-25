using Centuriin.CardGame.Core.Common.Commands.Rules;
using Centuriin.CardGame.Core.Common.Configuration;
using Centuriin.CardGame.Core.Common.Configuration.Pipeline;
using Centuriin.CardGame.Core.Common.ECS.Systems;
using Centuriin.CardGame.Core.Common.Events.Domain;

namespace Centuriin.CardGame.Core.Common.GameProfiles;

internal sealed class DefaultProfile : IGameProfile
{
    public static DefaultProfile Instance { get; } = new();

    public string Key => throw new NotSupportedException("Default profile not supported key.");

    private DefaultProfile() { }

    public void Configure(IConfigurableGamePipeline pipeline)
    {
        ArgumentNullException.ThrowIfNull(pipeline);

        _ = pipeline
            .Add<SetupTurnFlowSystem, GameStartedEvent>()
            .Add<DealerSystem, GameStartedEvent>()
            .Add<TurnFlowSystem, TurnFlowDefinedEvent>()
            .Add<CardMovementSystem, CardDealtEvent>();
    }

    public void Configure(IConfigurableCommandValidatation configurableCommandValidatation)
    {
        _ = configurableCommandValidatation
            .AddGeneralRule<ActorIsActivePlayerRule>();
    }
}
