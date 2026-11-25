using Centuriin.CardGame.Core.Engine.ECS.Systems;

namespace Centuriin.CardGame.Core.Engine.Loading.Configuration.Pipeline;

/// <summary>
/// Configures the order of game systems and their event subscriptions.
/// </summary>
public interface IConfigurableGamePipeline
{
    /// <summary>
    /// Adds the standard turn-flow, dealing, and card-movement systems.
    /// </summary>
    /// <returns>
    /// This configuration builder.
    /// </returns>
    public IConfigurableGamePipeline UseDefaultProfile();

    /// <summary>
    /// Appends a new system to the end of the pipeline.
    /// </summary>
    /// <typeparam name="TSystem">
    /// The game system type to create or configure.
    /// </typeparam>
    /// <returns>
    /// The configuration of the newly added system.
    /// </returns>
    public IConfigurableSystem Add<TSystem>()
        where TSystem : ISystem;

    /// <summary>
    /// Inserts a new system after an existing system.
    /// </summary>
    /// <typeparam name="TPrecedingSystem">
    /// The existing system after which the new system is inserted.
    /// </typeparam>
    /// <typeparam name="TSystem">
    /// The game system type to create or configure.
    /// </typeparam>
    /// <returns>
    /// The configuration of the newly added system.
    /// </returns>
    public IConfigurableSystem AddAfter<TPrecedingSystem, TSystem>()
        where TPrecedingSystem : ISystem
        where TSystem : ISystem;

    /// <summary>
    /// Inserts a new system before an existing system.
    /// </summary>
    /// <typeparam name="TFollowingSystem">
    /// The existing system before which the new system is inserted.
    /// </typeparam>
    /// <typeparam name="TSystem">
    /// The game system type to create or configure.
    /// </typeparam>
    /// <returns>
    /// The configuration of the newly added system.
    /// </returns>
    public IConfigurableSystem AddBefore<TFollowingSystem, TSystem>()
        where TFollowingSystem : ISystem
        where TSystem : ISystem;

    /// <summary>
    /// Replaces an existing system with a new system configuration.
    /// </summary>
    /// <typeparam name="TReplaceableSystem">
    /// The existing system to replace.
    /// </typeparam>
    /// <typeparam name="TSystem">
    /// The game system type to create or configure.
    /// </typeparam>
    /// <returns>
    /// The configuration of the newly added system.
    /// </returns>
    public IConfigurableSystem Replace<TReplaceableSystem, TSystem>()
        where TReplaceableSystem : ISystem
        where TSystem : ISystem;
}