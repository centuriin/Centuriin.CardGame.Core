namespace Centuriin.CardGame.Core.Common.Entities.Cards;

/// <summary>
/// Card entity.
/// </summary>
public sealed class Card : EntityBase, IEquatable<Card>
{
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

    /// <inheritdoc/>
    public bool Equals(Card? other) =>
        other is not null
        && other.Id == Id;

    /// <inheritdoc/>
    public override bool Equals(object? obj) => Equals(obj as Card);

    /// <inheritdoc/>
    public override int GetHashCode() => Id.GetHashCode();
}
