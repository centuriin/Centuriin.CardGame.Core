using System.Collections;

namespace Centuriin.CardGame.Core.Engine.ECS.Components;

/// <summary>
/// Components collection.
/// </summary>
public sealed class ComponentsCollection : IEnumerable<ComponentBase>
{
    private readonly Dictionary<Type, ComponentBase> _components = [];

    /// <summary>
    /// Gets a value indicating whether the collection is empty.
    /// </summary>
    public bool IsEmpty => _components.Count == 0;

    public ComponentsCollection() { }

    public ComponentsCollection(
        IEnumerable<ComponentBase> components)
    {
        ArgumentNullException.ThrowIfNull(components);
        _components = components.ToDictionary(k => k.GetType());
    }

    /// <summary>
    /// Sets or resets components.
    /// </summary>
    /// <param name="components">
    /// Components.
    /// </param>
    public void Set(params IEnumerable<ComponentBase> components)
    {
        ArgumentNullException.ThrowIfNull(components);

        foreach (var component in components)
        {
            _components[component.GetType()] = component;
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
    public T Get<T>() where T : ComponentBase
    {
        if (!_components.TryGetValue(typeof(T), out var component))
            throw new InvalidOperationException(
                $"Component {typeof(T).Name} not found.");

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
    public bool Has<T>() where T : ComponentBase =>
        _components.ContainsKey(typeof(T));

    /// <summary>
    /// Removes component by type.
    /// </summary>
    /// <typeparam name="T">
    /// Concrete type.
    /// </typeparam>
    public void Remove<T>() where T : ComponentBase
        => _components.Remove(typeof(T));

    /// <inheritdoc/>
    public IEnumerator<ComponentBase> GetEnumerator() =>
        _components.Values.GetEnumerator();

    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}