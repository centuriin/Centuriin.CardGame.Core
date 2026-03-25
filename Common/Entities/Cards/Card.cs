using Centuriin.CardGame.Core.Common.Components;

namespace Centuriin.CardGame.Core.Common.Entities.Cards;

/// <summary>
/// Card entity.
/// </summary>
public sealed class Card : IEquatable<Card>
{
    private readonly Dictionary<Type, IComponent> _components = [];

    /// <summary>
    /// Instance id.
    /// </summary>
    public CardId Id { get; }

    /// <summary>
    /// Creates new instance of <see cref="Card"/>.
    /// </summary>
    /// <param name="id">
    /// Instance identifier.
    /// </param>
    public Card(CardId id)
    {
        Id = id;
    }

    /// <summary>
    /// Adds component to card.
    /// </summary>
    /// <param name="component">
    /// Component.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// When <paramref name="component"/> is <see langword="null"/>.
    /// </exception>
    public void Add(IComponent component)
    {
        ArgumentNullException.ThrowIfNull(component);

        _components[component.GetType()] = component;
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
        if (!_components.TryGetValue(typeof(T), out var component))
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
        where T : class, IComponent => _components.ContainsKey(typeof(T));

    /// <summary>
    /// Removes component by type.
    /// </summary>
    /// <typeparam name="T">
    /// Concrete type.
    /// </typeparam>
    public void Remove<T>() 
        where T : class, IComponent => _components.Remove(typeof(T));

    /// <inheritdoc/>
    public bool Equals(Card? other) =>
        other is not null
        && other.Id == Id;

    /// <inheritdoc/>
    public override bool Equals(object? obj) => Equals(obj as Card);

    /// <inheritdoc/>
    public override int GetHashCode() => Id.GetHashCode();
}
