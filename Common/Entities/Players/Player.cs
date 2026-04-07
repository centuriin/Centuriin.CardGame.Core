using Centuriin.CardGame.Core.Common.Components.Players;

namespace Centuriin.CardGame.Core.Common.Entities.Players;

public sealed class Player : EntityBase<PlayerId>, IEquatable<Player>
{
    public static Player System => new(PlayerId.System)
    {
        Components =
        {
            { typeof(PlayerRoleComponent), new PlayerRoleComponent(PlayerRole.Bank) }
        }
    };

    public Player(PlayerId id) : base(id)
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
}