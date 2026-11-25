namespace Centuriin.CardGame.Core.Engine.ECS.Systems;

/// <summary>
/// Creates game systems and their dependencies.
/// </summary>
public interface ISystemFactory
{
    /// <summary>
    /// Creates an instance of a game system.
    /// </summary>
    /// <typeparam name="TSystem">
    /// The game system type to create or configure.
    /// </typeparam>
    /// <returns>
    /// The created instance.
    /// </returns>
    public TSystem Create<TSystem>() where TSystem : ISystem;
}