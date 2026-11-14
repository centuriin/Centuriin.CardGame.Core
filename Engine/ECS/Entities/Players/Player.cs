using Centuriin.CardGame.Core.Engine.ECS.Components;
using Centuriin.CardGame.Core.Engine.ECS.Components.Players;

namespace Centuriin.CardGame.Core.Engine.ECS.Entities.Players;

public sealed class Player : EntityBase, IEquatable<Player>
{
    public static Player System { get; } =
        new(
            EntityId.Default,
            [
                new PlayerRoleComponent(PlayerRole.Bank),
                PlayerIdentifierComponent.System,
            ]);

    public Player(EntityId id)
        : base(id) { }

    public Player(EntityId id, IEnumerable<ComponentBase> components)
        : base(id, components) { }

    private Player(EntityId id, ComponentsCollection components)
        : base(id, components) { }

    /// <inheritdoc/>
    public override Player Add(params IEnumerable<ComponentBase> components)
    {
        ArgumentNullException.ThrowIfNull(components);

        var changedCollection = Components.Add(components);

        return new(Id, changedCollection);
    }

    /// <inheritdoc/>
    public override Player Replace<T>(T replacement)
    {
        ArgumentNullException.ThrowIfNull(replacement);

        var changedCollection = Components.Replace(replacement);

        return new(Id, changedCollection);
    }

    /// <inheritdoc/>
    public override Player Remove<T>()
    {
        var changedCollection = Components.Remove<T>();

        return new(Id, changedCollection);
    }

    /// <inheritdoc/>
    public bool Equals(Player? other) =>
        other is not null
        && Id == other.Id;

    /// <inheritdoc/>
    public override bool Equals(object? obj) => Equals(obj as Player);

    /// <inheritdoc/>
    public override int GetHashCode() => Id.GetHashCode();
}