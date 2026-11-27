using Centuriin.CardGame.Core.Application.Initialization.Configuration;
using Centuriin.CardGame.Core.Engine.Loading.Configuration;

namespace Centuriin.CardGame.Core.Application.Modulation;

/// <summary>
/// Describes a game module and declares its dependencies.
/// </summary>
public interface IGameModule
{
    /// <summary>
    /// Gets the unique key used for the module's core extension points.
    /// </summary>
    public string Key { get; }

    /// <summary>
    /// Gets the engine profile of the module.
    /// </summary>
    public IGameProfile GameProfile { get; }

    /// <summary>
    /// Declares the module's ordinary scoped dependencies.
    /// </summary>
    /// <param name="dependencies">
    /// The dependency builder.
    /// </param>
    public void Configure(IConfigurableModuleDependencies dependencies);

    /// <summary>
    /// Declares the entity initializers in execution order.
    /// </summary>
    /// <param name="initializers">
    /// The initializer builder.
    /// </param>
    public void Configure(IConfigurableEntityInitializers initializers);
}
