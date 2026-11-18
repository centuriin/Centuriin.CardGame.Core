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

    /// <inheritdoc/>
    public bool Equals(Card? other) =>
        other is not null
        && other.Id == Id;

    /// <inheritdoc/>
    public override bool Equals(object? obj) => Equals(obj as Card);

    /// <inheritdoc/>
    public override int GetHashCode() => Id.GetHashCode();
}
