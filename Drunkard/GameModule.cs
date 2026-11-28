using Centuriin.CardGame.Core.Application.Initialization;
using Centuriin.CardGame.Core.Application.Modulation;
using Centuriin.CardGame.Core.Drunkard.Initialization;
using Centuriin.CardGame.Core.Engine.Loading.Configuration;

namespace Centuriin.CardGame.Core.Drunkard;

/// <summary>
/// Configures dependencies, initialization, and the engine profile of Drunkard.
/// </summary>
public sealed class GameModule : IGameModule
{
    /// <inheritdoc/>
    public string Key => "drunkard";

    /// <inheritdoc/>
    public IGameProfile GameProfile { get; } = new GameProfile();

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
            .Add<DeckInitializer>();
    }
}
