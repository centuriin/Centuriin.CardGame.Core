using Centuriin.CardGame.Core.Common.Components;

namespace Centuriin.CardGame.Core.Common.Entities;

public abstract class EntityBase
{
    protected Dictionary<Type, ComponentBase> Components { get; init; } = [];

    /// <summary>
    /// Adds component.
    /// </summary>
    /// <param name="component">
    /// Component.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// When <paramref name="component"/> is <see langword="null"/>.
    /// </exception>
    public void Add(ComponentBase component)
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
        where T : ComponentBase
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
        where T : ComponentBase => Components.ContainsKey(typeof(T));

    /// <summary>
    /// Removes component by type.
    /// </summary>
    /// <typeparam name="T">
    /// Concrete type.
    /// </typeparam>
    public void Remove<T>()
        where T : ComponentBase => Components.Remove(typeof(T));
}
