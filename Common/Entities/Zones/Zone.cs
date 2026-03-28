namespace Centuriin.CardGame.Core.Common.Entities.Zones;

public sealed class Zone : EntityBase<ZoneId>, IEquatable<Zone>
{
    public bool IsEmpty => Components.Any();

    public Zone(ZoneId id) : base(id)
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
