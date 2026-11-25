using Centuriin.CardGame.Core.Engine.ECS.Components;

namespace Centuriin.CardGame.Core.Engine.ECS.Entities.Zones;

/// <summary>
/// Represents a card location, such as a deck, hand, or table slot.
/// </summary>
public sealed class Zone : EntityBase, IEquatable<Zone>
{
    /// <summary>
    /// Initializes a new <see cref="Zone"/> instance.
    /// </summary>
    /// <param name="id">
    /// The entity identifier.
    /// </param>
    public Zone(EntityId id) : base(id)
    {
    }

    /// <summary>
    /// Initializes a new <see cref="Zone"/> instance.
    /// </summary>
    /// <param name="id">
    /// The entity identifier.
    /// </param>
    /// <param name="components">
    /// The components assigned to the entity or template.
    /// </param>
    public Zone(EntityId id, IEnumerable<ComponentBase> components)
        : base(id, components)
    {
    }

    /// <inheritdoc/>
    public bool Equals(Zone? other) =>
        other is not null
        && Id == other.Id;

    /// <inheritdoc/>
    public override bool Equals(object? obj) => Equals(obj as Zone);

    /// <inheritdoc/>
    public override int GetHashCode() => Id.GetHashCode();
}
