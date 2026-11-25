namespace Centuriin.CardGame.Core.Engine.ECS.Entities;

/// <summary>
/// Entity id.
/// </summary>
/// <param name="Value">
/// Value.
/// </param>
public readonly record struct EntityId(int Value) : IComparable<EntityId>
{
    /// <summary>
    /// Gets the reserved zero identifier used by the system player.
    /// </summary>
    public static EntityId Default { get; }

    /// <inheritdoc/>
    public int CompareTo(EntityId other) => Value.CompareTo(other.Value);

    /// <summary>
    /// Returns the next sequential value.
    /// </summary>
    /// <param name="id">
    /// The entity identifier.
    /// </param>
    /// <returns>
    /// The resulting value.
    /// </returns>
    public static EntityId operator ++(EntityId id) => new(id.Value + 1);
}
