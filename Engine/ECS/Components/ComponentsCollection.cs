using System.Collections;
using System.Collections.Immutable;

namespace Centuriin.CardGame.Core.Engine.ECS.Components;

/// <summary>
/// Components collection.
/// </summary>
public sealed class ComponentsCollection : IEnumerable<ComponentBase>
{
    private readonly ImmutableDictionary<Type, ComponentBase> _components = [];

    /// <summary>
    /// Gets a value indicating whether the collection is empty.
    /// </summary>
    public bool IsEmpty { get; }

    public ComponentsCollection() { }

    public ComponentsCollection(
        IEnumerable<ComponentBase> components)
    {
        ArgumentNullException.ThrowIfNull(components);
        _components = components.ToImmutableDictionary(k => k.GetType());
    }

    private ComponentsCollection(ImmutableDictionary<Type, ComponentBase> components)
    {
        _components = components;
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
    public ComponentsCollection Add(params IEnumerable<ComponentBase> components)
    {
        ArgumentNullException.ThrowIfNull(components);

        var changedCollection = _components.AddRange(
            components.Select(x =>
                new KeyValuePair<Type, ComponentBase>(x.GetType(), x)));

        return new ComponentsCollection(changedCollection);
    }

    /// <summary>
    /// Replaces component.
    /// </summary>
    /// <typeparam name="T">
    /// Component type.
    /// </typeparam>
    /// <param name="replacement">
    /// Replaceable component.
    /// </param>
    public ComponentsCollection Replace<T>(T replacement) where T : ComponentBase
    {
        ArgumentNullException.ThrowIfNull(replacement);

        var componentType = typeof(T);

        if (!_components.ContainsKey(componentType))
            throw new InvalidOperationException(
                $"Component {componentType.Name} not found.");

        var changedCollection = _components.SetItem(componentType, replacement);

        return new ComponentsCollection(changedCollection);
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
    public ComponentsCollection Remove<T>() where T : ComponentBase
    {
        var changedCollection = _components.Remove(typeof(T));

        return new ComponentsCollection(changedCollection);
    }

    /// <inheritdoc/>
    public IEnumerator<ComponentBase> GetEnumerator() => 
        _components.Values.GetEnumerator();
    
    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}