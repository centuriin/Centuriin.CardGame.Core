using Centuriin.CardGame.Core.Engine.ECS.Components;
using Centuriin.CardGame.Core.Engine.ECS.Components.Players;

namespace Centuriin.CardGame.Core.Engine.ECS.Entities.Players;

/// <summary>
/// Represents a player entity identified independently of its external player identifier.
/// </summary>
public sealed class Player : EntityBase, IEquatable<Player>
{
    /// <summary>
    /// Creates a system player entity that owns the shared deck.
    /// </summary>
    public static Player System =>
        new(
            EntityId.Default,
            [
                new PlayerRoleComponent(PlayerRole.Bank), 
                PlayerIdentifierComponent.System
            ]);

    /// <summary>
    /// Initializes a new <see cref="Player"/> instance.
    /// </summary>
    /// <param name="id">
    /// The entity identifier.
    /// </param>
    public Player(EntityId id)
        : base(id) { }

    /// <summary>
    /// Initializes a new <see cref="Player"/> instance.
    /// </summary>
    /// <param name="id">
    /// The entity identifier.
    /// </param>
    /// <param name="components">
    /// The components assigned to the entity or template.
    /// </param>
    public Player(EntityId id, IEnumerable<ComponentBase> components)
        : base(id, components) { }

    /// <inheritdoc/>
    public bool Equals(Player? other) =>
        other is not null
        && Id == other.Id;

    /// <inheritdoc/>
    public override bool Equals(object? obj) => Equals(obj as Player);

    /// <inheritdoc/>
    public override int GetHashCode() => Id.GetHashCode();
}