namespace Centuriin.CardGame.Core.Common.ECS.Entities.Cards;

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

    /// <inheritdoc/>
    public bool Equals(Card? other) =>
        other is not null
        && other.Id == Id;

    /// <inheritdoc/>
    public override bool Equals(object? obj) => Equals(obj as Card);

    /// <inheritdoc/>
    public override int GetHashCode() => Id.GetHashCode();

    /// <inheritdoc/>
    public override Card Copy()
    {
        var copy = new Card(Id);
        copy.Add(Components.Values.Select(x => x.Copy()));

        return copy;
    }

    public override TEntity Copy<TEntity>() => throw new NotImplementedException();
}
