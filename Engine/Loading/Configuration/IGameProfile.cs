using Centuriin.CardGame.Core.Engine.Loading.Configuration.Commands;
using Centuriin.CardGame.Core.Engine.Loading.Configuration.Pipeline;

namespace Centuriin.CardGame.Core.Engine.Loading.Configuration;

/// <summary>
/// Defines a game module key, event pipeline, and command validation configuration.
/// </summary>
public interface IGameProfile
{
    /// <summary>
    /// Gets the module key used to resolve game-specific dependencies.
    /// </summary>
    public string Key { get; }

    /// <summary>
    /// Configures the game's systems and their selected event subscriptions.
    /// </summary>
    /// <param name="configurableGamePipeline">
    /// The builder used to configure game systems and event subscriptions.
    /// </param>
    public void Configure(IConfigurableGamePipeline configurableGamePipeline);

    /// <summary>
    /// Configures the validation rules for commands supported by the game.
    /// </summary>
    /// <param name="configurableCommandValidations">
    /// The builder used to configure command validation rules.
    /// </param>
    public void Configure(IConfigurableCommandValidator configurableCommandValidations);
}
