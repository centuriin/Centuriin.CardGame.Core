using Centuriin.CardGame.Core.Engine.ECS.Components;
using Centuriin.CardGame.Core.Engine.ECS.Components.Players;

namespace Centuriin.CardGame.Core.Engine.ECS.Entities.Players;

public sealed class Player : EntityBase, IEquatable<Player>
{
    public static Player System { get; } =
        new(
            EntityId.Default,
            (IEnumerable<ComponentBase>)
            [
                new PlayerRoleComponent(PlayerRole.Bank), 
                PlayerIdentifierComponent.System
            ]);

    public Player(EntityId id)
        : base(id) { }

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