using Centuriin.CardGame.Core.Common.Components;

namespace Centuriin.CardGame.Core.Common.Entities;

public abstract class EntityBase<TId>
    where TId : struct, IEquatable<TId>
{
    /// <summary>
    /// Instance id.
    /// </summary>
    public TId Id { get; }

    protected Dictionary<Type, IGameComponent> Components { get; } = [];

    protected EntityBase(TId id)
    {
        Id = id;
    }

    /// <summary>
    /// Adds component.
    /// </summary>
    /// <param name="component">
    /// Component.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// When <paramref name="component"/> is <see langword="null"/>.
    /// </exception>
    public void Add(IGameComponent component)
    {
        ArgumentNullException.ThrowIfNull(component);

        Components[component.GetType()] = component;
    }

    /// <summary>
    /// Gets component by type.
    /// </summary>
    /// <typeparam name="T">
    /// Concrete component type.
    /// </typeparam>
    /// <returns>
    /// Component of <typeparamref name="T"/>.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// When component not found.
    /// </exception>
    public T Get<T>()
    {
        if (!Components.TryGetValue(typeof(T), out var component))
            throw new InvalidOperationException($"Component {typeof(T).Name} not found.");

        return (T)component;
    }

    /// <summary>
    /// Checks component contains.
    /// </summary>
    /// <typeparam name="T">
    /// Concrete type of component.
    /// </typeparam>
    /// <returns>
    /// <see langword="true"/> if exists, otherwise - <see langword="false"/>.
    /// </returns>
    public bool Has<T>()
        where T : class, IGameComponent => Components.ContainsKey(typeof(T));

    /// <summary>
    /// Removes component by type.
    /// </summary>
    /// <typeparam name="T">
    /// Concrete type.
    /// </typeparam>
    public void Remove<T>()
        where T : class, IGameComponent => Components.Remove(typeof(T));
}
