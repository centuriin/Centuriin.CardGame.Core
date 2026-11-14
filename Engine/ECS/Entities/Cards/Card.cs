using Centuriin.CardGame.Core.Engine.ECS.Components;

namespace Centuriin.CardGame.Core.Engine.ECS.Entities.Cards;

/// <summary>
/// Card entity.
/// </summary>
public sealed class Card : EntityBase, IEquatable<Card>
{
    /// <summary>
    /// Creates new instance of <see cref="Card"/>.
    /// </summary>
    /// <param name="id">
    /// Instance identifier.
    /// </param>
    public Card(EntityId id) : base(id)
    {
    }

    public Card(EntityId id, IEnumerable<ComponentBase> components)
        : base(id, components)
    {
    }

    private Card(EntityId id, ComponentsCollection components)
        : base(id, components)
    {

    }

    /// <inheritdoc/>
    public override Card Add(params IEnumerable<ComponentBase> components)
    {
        ArgumentNullException.ThrowIfNull(components);

        var changedCollection = Components.Add(components);

        return new(Id, changedCollection);
    }

    /// <inheritdoc/>
    public override Card Replace<T>(T replacement)
    {
        ArgumentNullException.ThrowIfNull(replacement);

        var changedCollection = Components.Replace(replacement);

        return new(Id, changedCollection);
    }

    /// <inheritdoc/>
    public override Card Remove<T>()
    {
        var changedCollection = Components.Remove<T>();

        return new(Id, changedCollection);
    }

    /// <inheritdoc/>
    public bool Equals(Card? other) =>
        other is not null
        && other.Id == Id;

    /// <inheritdoc/>
    public override bool Equals(object? obj) => Equals(obj as Card);

    /// <inheritdoc/>
    public override int GetHashCode() => Id.GetHashCode();
}
