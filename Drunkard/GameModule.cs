using Centuriin.CardGame.Core.Application.Initialization;
using Centuriin.CardGame.Core.Application.Modulation;
using Centuriin.CardGame.Core.Engine.Commands.Rules;
using Centuriin.CardGame.Core.Engine.Loading.Configuration;
using Centuriin.CardGame.Core.Engine.Loading.Configuration.Commands;
using Centuriin.CardGame.Core.Engine.Loading.Configuration.Pipeline;

namespace Centuriin.CardGame.Core.Drunkard;

/// <summary>
/// Configures dependencies, initialization, and the engine profile of Drunkard.
/// </summary>
public sealed class GameModule : IGameModule, IGameProfile
{
    /// <summary>
    /// The dependency injection key used by the Drunkard module.
    /// </summary>
    internal const string KEY = "drunkard";

    /// <inheritdoc/>
    public string Key => KEY;

    /// <inheritdoc/>
    public IGameProfile GameProfile => this;


    /// <inheritdoc/>
    public void Configure(IConfigurableModuleDependencies dependencies) => 
        ArgumentNullException.ThrowIfNull(dependencies);

    /// <inheritdoc/>
    public void Configure(IConfigurableEntityInitializers initializers)
    {
        ArgumentNullException.ThrowIfNull(initializers);

        _ = initializers
            .Add<DefaultPlayersInitializer>()
            .Add<DefaultZonesInitializer>()
            .Add<DefaultDecksInitializer>();
    }

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
