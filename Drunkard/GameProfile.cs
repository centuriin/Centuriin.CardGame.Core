using Centuriin.CardGame.Core.Engine.Commands.Rules;
using Centuriin.CardGame.Core.Engine.Loading.Configuration;
using Centuriin.CardGame.Core.Engine.Loading.Configuration.Commands;
using Centuriin.CardGame.Core.Engine.Loading.Configuration.Pipeline;

namespace Centuriin.CardGame.Core.Drunkard;

/// <summary>
/// Configures the systems and command validation used by the Drunkard prototype.
/// </summary>
public sealed class GameProfile : IGameProfile
{
    /// <summary>
    /// The dependency injection key used by the Drunkard module.
    /// </summary>
    internal const string KEY = "drunkard";

    /// <summary>
    /// Gets the shared instance.
    /// </summary>
    public static GameProfile Instance { get; } = new();

    /// <inheritdoc/>
    public string Key => KEY;

    private GameProfile() { }

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
