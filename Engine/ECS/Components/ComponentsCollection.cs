using System.Collections;

namespace Centuriin.CardGame.Core.Engine.ECS.Components;

/// <summary>
/// Stores one component per concrete component type.
/// </summary>
public sealed class ComponentsCollection : IEnumerable<ComponentBase>
{
    private readonly Dictionary<Type, ComponentBase> _components = [];

    /// <summary>
    /// Gets a value indicating whether the collection is empty.
    /// </summary>
    public bool IsEmpty => _components.Count == 0;

    /// <summary>
    /// Initializes a new <see cref="ComponentsCollection"/> instance.
    /// </summary>
    public ComponentsCollection() { }

    /// <summary>
    /// Initializes a new <see cref="ComponentsCollection"/> instance.
    /// </summary>
    /// <param name="components">
    /// The components assigned to the entity or template.
    /// </param>
    public ComponentsCollection(
        IEnumerable<ComponentBase> components)
    {
        ArgumentNullException.ThrowIfNull(components);
        _components = components.ToDictionary(k => k.GetType());
    }

    /// <summary>
    /// Adds components, replacing any existing components of the same concrete types.
    /// </summary>
    /// <param name="components">
    /// The components to add or replace.
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
    /// Retrieves the component of the requested concrete type.
    /// </summary>
    /// <typeparam name="T">
    /// Concrete component type.
    /// </typeparam>
    /// <returns>
    /// Component of <typeparamref name="T"/>.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// The requested component is not present.
    /// </exception>
    public T Get<T>() where T : ComponentBase
    {
        if (!_components.TryGetValue(typeof(T), out var component))
            throw new InvalidOperationException(
                $"Component {typeof(T).Name} not found.");

        return (T)component;
    }

    /// <summary>
    /// Checks whether a component of the requested concrete type is present.
    /// </summary>
    /// <typeparam name="T">
    /// Concrete type of component.
    /// </typeparam>
    /// <returns>
    /// <see langword="true"/> when the component exists; otherwise, <see langword="false"/>.
    /// </returns>
    public bool Has<T>() where T : ComponentBase =>
        _components.ContainsKey(typeof(T));

    /// <summary>
    /// Removes the component of the requested concrete type, when present.
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
