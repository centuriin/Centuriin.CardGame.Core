namespace Centuriin.CardGame.Core.Application.Modulation;

/// <summary>
/// Declares ordinary scoped dependencies of a game module.
/// </summary>
public interface IConfigurableModuleDependencies
{
    /// <summary>
    /// Registers a dependency by its concrete type.
    /// </summary>
    /// <typeparam name="TImplementation">
    /// The dependency type.
    /// </typeparam>
    /// <returns>
    /// This dependency builder.
    /// </returns>
    public IConfigurableModuleDependencies Add<TImplementation>()
        where TImplementation : class;

    /// <summary>
    /// Registers an implementation for a dependency contract.
    /// </summary>
    /// <typeparam name="TService">
    /// The dependency contract.
    /// </typeparam>
    /// <typeparam name="TImplementation">
    /// The implementation type.
    /// </typeparam>
    /// <returns>
    /// This dependency builder.
    /// </returns>
    public IConfigurableModuleDependencies Add<TService, TImplementation>()
        where TService : class
        where TImplementation : class, TService;
}
