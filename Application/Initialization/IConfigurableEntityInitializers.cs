namespace Centuriin.CardGame.Core.Application.Initialization;

/// <summary>
/// Declares the entity initializers of a game module in execution order.
/// </summary>
public interface IConfigurableEntityInitializers
{
    /// <summary>
    /// Appends a scoped entity initializer associated with the module.
    /// </summary>
    /// <typeparam name="TInitializer">
    /// The entity initializer type.
    /// </typeparam>
    /// <returns>
    /// This initializer builder.
    /// </returns>
    public IConfigurableEntityInitializers Add<TInitializer>()
        where TInitializer : class, IEntityInitializer;
}
