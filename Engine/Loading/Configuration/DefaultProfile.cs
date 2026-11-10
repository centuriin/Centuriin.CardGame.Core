using Centuriin.CardGame.Core.Engine.Commands.Rules;
using Centuriin.CardGame.Core.Engine.ECS.Systems;
using Centuriin.CardGame.Core.Engine.Events.Domain;
using Centuriin.CardGame.Core.Engine.Loading.Configuration.Commads;
using Centuriin.CardGame.Core.Engine.Loading.Configuration.Pipeline;

namespace Centuriin.CardGame.Core.Engine.Loading.Configuration;

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
                .ForEvent<TurnEndedEvent>()
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
