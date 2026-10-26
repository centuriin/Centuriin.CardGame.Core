using Centuriin.CardGame.Core.Common.Commands.Rules;
using Centuriin.CardGame.Core.Common.Configuration.Commads;
using Centuriin.CardGame.Core.Common.Configuration.Pipeline;
using Centuriin.CardGame.Core.Common.ECS.Systems;
using Centuriin.CardGame.Core.Common.Events.Domain;

namespace Centuriin.CardGame.Core.Common.GameProfiles;

internal sealed class DefaultProfile : IGameProfile
{
    public static DefaultProfile Instance { get; } = new();

    public string Key => throw new NotSupportedException("Default profile not supported key.");

    private DefaultProfile() { }

    /// <inheritdoc/>
    public void Configure(IConfigurableGamePipeline configurableGamePipeline)
    {
        ArgumentNullException.ThrowIfNull(configurableGamePipeline);

        _ = configurableGamePipeline
            .Add<SetupTurnFlowSystem>()
                .ForEvent<GameStartedEvent>()
                .GamePipeline

            .Add<DealerSystem>()
                .ForEvent<GameStartedEvent>()
                .GamePipeline

            .Add<TurnFlowSystem>()
                .ForEvent<TurnFlowDefinedEvent>()
                .GamePipeline

            .Add<CardMovementSystem>()
                .ForEvent<CardDealtEvent>();
    }

    /// <inheritdoc/>
    public void Configure(IConfigurableCommandValidator configurableCommandValidator)
    {
        ArgumentNullException.ThrowIfNull(configurableCommandValidator);

        _ = configurableCommandValidator
            .AddGeneralRule<ActorIsActivePlayerRule>();
    }
}
