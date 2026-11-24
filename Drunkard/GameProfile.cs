using Centuriin.CardGame.Core.Engine.Commands.Rules;
using Centuriin.CardGame.Core.Engine.Loading.Configuration;
using Centuriin.CardGame.Core.Engine.Loading.Configuration.Commads;
using Centuriin.CardGame.Core.Engine.Loading.Configuration.Pipeline;

namespace Centuriin.CardGame.Core.Drunkard;

public sealed class GameProfile : IGameProfile
{
    internal const string KEY = "drunkard";

    public static GameProfile Instance { get; } = new();

    public string Key => KEY;

    private GameProfile() { }

    public void Configure(IConfigurableGamePipeline configurableGamePipeLine)
    {
        ArgumentNullException.ThrowIfNull(configurableGamePipeLine);

        _ = configurableGamePipeLine
            .UseDefaultProfile();
    }

    public void Configure(IConfigurableCommandValidator configurableCommandValidations)
    {
        ArgumentNullException.ThrowIfNull(configurableCommandValidations);

        _ = configurableCommandValidations
            .AddGeneralRule<ActorIsActivePlayerRule>()
            .AddValidation<FakeCommand>()
                .AddRule<ActorIsActivePlayerRule>();
    }
}
