using Centuriin.CardGame.Core.Engine.ECS.Components.Players;

namespace Centuriin.CardGame.Core.Engine.ECS.Entities.Players;

public sealed class Player : EntityBase, IEquatable<Player>
{
    public static Player System => new(EntityId.Default)
    {
        Components =
        {
            { typeof(PlayerRoleComponent), new PlayerRoleComponent(PlayerRole.Bank) },
            { typeof(PlayerIdentifierComponent), PlayerIdentifierComponent.System }
        }
    };

    public Player(EntityId id) : base(id)
    {
    }

    /// <inheritdoc/>
    public bool Equals(Player? other) =>
        other is not null
        && Id == other.Id;

    /// <inheritdoc/>
    public override bool Equals(object? obj) => Equals(obj as Player);

    /// <inheritdoc/>
    public override int GetHashCode() => Id.GetHashCode();

    public override Player Copy()
    {
        var copy = new Player(Id);
        copy.Add(Components.Values.Select(x => x.Copy()));

        return copy;
    }
}