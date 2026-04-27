using Centuriin.CardGame.Core.Common.Components;

namespace Centuriin.CardGame.Core.Common.Entities;

/// <summary>
/// Base class for entity.
/// </summary>
public abstract class EntityBase
{
    /// <summary>
    /// Instance id.
    /// </summary>
    public EntityId Id { get; }

    protected Dictionary<Type, ComponentBase> Components { get; init; } = [];

    protected EntityBase(EntityId id)
    {
        Id = id;
    }

    /// <summary>
    /// Adds component.
    /// </summary>
    /// <param name="components">
    /// Component.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// When <paramref name="components"/> is <see langword="null"/>.
    /// </exception>
    public void Add(params IReadOnlyCollection<ComponentBase> components)
    {
        ArgumentNullException.ThrowIfNull(components);

        foreach (var component in components)
        {
            Components[component.GetType()] = component;
        }
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
