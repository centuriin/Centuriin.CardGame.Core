using Centuriin.CardGame.Core.Engine.Commands.Rules;
using Centuriin.CardGame.Core.Engine.ECS.Systems;
using Centuriin.CardGame.Core.Engine.Events.Domain;
using Centuriin.CardGame.Core.Engine.Loading.Configuration.Commands;
using Centuriin.CardGame.Core.Engine.Loading.Configuration.Pipeline;

namespace Centuriin.CardGame.Core.Engine.Loading.Configuration;

/// <summary>
/// Configures the standard dealing, card movement, and turn-flow systems.
/// </summary>
internal sealed class DefaultProfile : IGameProfile
{
    private DefaultProfile() { }

    /// <summary>
    /// Gets the shared instance.
    /// </summary>
    public static DefaultProfile Instance { get; } = new();

    /// <inheritdoc/>
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
    /// <inheritdoc/>
    public void Configure(IConfigurableCommandValidator configurableCommandValidations)
    {
        ArgumentNullException.ThrowIfNull(configurableCommandValidations);

        configurableCommandValidations
            .AddGeneralRule<ActorIsActivePlayerRule>();
    }
}
