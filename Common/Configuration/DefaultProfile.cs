using Centuriin.CardGame.Core.Common.Commands.Rules;
using Centuriin.CardGame.Core.Common.Configuration.Commads;
using Centuriin.CardGame.Core.Common.Configuration.Pipeline;
using Centuriin.CardGame.Core.Common.ECS.Systems;
using Centuriin.CardGame.Core.Common.Events.Domain;

namespace Centuriin.CardGame.Core.Common.Configuration;

internal sealed class DefaultProfile : IGameProfile
{
    private DefaultProfile() { }

    public static DefaultProfile Instance { get; } = new();

    public string Key => 
        throw new NotSupportedException("Default profile not supported key.");

    public void Configure(IConfigurableGamePipeline configurableGamePipeline)
    {
        ArgumentNullException.ThrowIfNull(configurableGamePipeline);

        _ = configurableGamePipeline
            .Add<SetupTurnFlowSystem>()
                .ForEvent<GameStartedEvent>()
                .Pipeline

            .Add<DealerSystem>()
                .ForEvent<GameStartedEvent>()
                .Pipeline

            .Add<TurnFlowSystem>()
                .ForEvent<TurnFlowDefinedEvent>()
                .Pipeline

            .Add<CardMovementSystem>()
                .ForEvent<CardDealtEvent>();
    }
    public void Configure(IConfigurableCommandValidator configurableCommandValidations)
    {
        ArgumentNullException.ThrowIfNull(configurableCommandValidations);

        configurableCommandValidations
            .AddGeneralRule<ActorIsActivePlayerRule>();
    }
}
