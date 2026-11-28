using Centuriin.CardGame.Core.Engine.Commands.Rules;
using Centuriin.CardGame.Core.Engine.Loading.Configuration;
using Centuriin.CardGame.Core.Engine.Loading.Configuration.Commands;
using Centuriin.CardGame.Core.Engine.Loading.Configuration.Pipeline;

namespace Centuriin.CardGame.Core.Drunkard;

internal sealed class GameProfile : IGameProfile
{
    /// <inheritdoc/>
    public void Configure(IConfigurableGamePipeline configurableGamePipeline)
    {
        ArgumentNullException.ThrowIfNull(configurableGamePipeline);

        _ = configurableGamePipeline
            .UseDefaultProfile();
    }

    /// <inheritdoc/>
    public void Configure(IConfigurableCommandValidator configurableCommandValidations)
    {
        ArgumentNullException.ThrowIfNull(configurableCommandValidations);

        _ = configurableCommandValidations
            .AddGeneralRule<ActorIsActivePlayerRule>()
            .AddValidation<FakeCommand>()
                .AddRule<ActorIsActivePlayerRule>();
    }
}
