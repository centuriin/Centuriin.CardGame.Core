using System.Collections;

namespace Centuriin.CardGame.Core.Engine.ECS.Components;

/// <summary>
/// Components collection.
/// </summary>
internal sealed class ComponentsCollection : IComponentsCollection
{
    private readonly Dictionary<Type, ComponentBase> _components = [];

    /// <inheritdoc/>
    public bool IsEmpty { get; }

    public ComponentsCollection() { }

    public ComponentsCollection(
        IEnumerable<ComponentBase> components)
    {
        ArgumentNullException.ThrowIfNull(components);
        _components = components.ToDictionary(k => k.GetType());
    }

    /// <inheritdoc/>
    public void Add(params IEnumerable<ComponentBase> components)
    {
        ArgumentNullException.ThrowIfNull(components);

        foreach (var component in components)
        {
            _components[component.GetType()] = component;
        }
    }

    /// <inheritdoc/>
    public void Replace<T>(T replacement) where T : ComponentBase
    {
        ArgumentNullException.ThrowIfNull(replacement);

        var componentType = typeof(T);

        if (!_components.ContainsKey(componentType))
            throw new InvalidOperationException(
                $"Component {componentType.Name} not found.");

        _components[componentType] = replacement;
    }

    /// <inheritdoc/>
    public T Get<T>() where T : ComponentBase
    {
        if (!_components.TryGetValue(typeof(T), out var component))
            throw new InvalidOperationException(
                $"Component {typeof(T).Name} not found.");

        return (T)component;
    }

    /// <inheritdoc/>
    public bool Has<T>() where T : ComponentBase => 
        _components.ContainsKey(typeof(T));

    /// <inheritdoc/>
    public void Remove<T>() where T : ComponentBase => 
        _components.Remove(typeof(T));

    /// <inheritdoc/>
    public IEnumerator<ComponentBase> GetEnumerator() => 
        _components.Values.GetEnumerator();
    
    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}