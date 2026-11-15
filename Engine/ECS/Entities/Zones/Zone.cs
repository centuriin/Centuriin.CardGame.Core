using Centuriin.CardGame.Core.Engine.ECS.Components;

namespace Centuriin.CardGame.Core.Engine.ECS.Entities.Zones;

public sealed class Zone : EntityBase, IEquatable<Zone>
{
    public Zone(EntityId id) : base(id)
    {
    }

    public Zone(EntityId id, IEnumerable<ComponentBase> components)
        : base(id, components)
    {
    }

    /// <inheritdoc/>
    public override Zone Add(params IEnumerable<ComponentBase> components)
    {
        ArgumentNullException.ThrowIfNull(components);

        var changedCollection = Components.Add(components);

        return new(Id, changedCollection);
    }

    /// <inheritdoc/>
    public override Zone Replace(params IEnumerable<ComponentBase> components)
    {
        ArgumentNullException.ThrowIfNull(components);

        var changedCollection = Components.Replace(components);

        return new(Id, changedCollection);
    }

    /// <inheritdoc/>
    public override Zone Remove<T>()
    {
        var changedCollection = Components.Remove<T>();

        return new(Id, changedCollection);
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
