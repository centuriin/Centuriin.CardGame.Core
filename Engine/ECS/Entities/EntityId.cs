namespace Centuriin.CardGame.Core.Engine.ECS.Entities;

/// <summary>
/// Entity id.
/// </summary>
/// <param name="Value">
/// Value.
/// </param>
public readonly record struct EntityId(int Value) : IComparable<EntityId>
{
    public static EntityId Default { get; }

    public int CompareTo(EntityId other) => Value.CompareTo(other.Value);

    public static EntityId operator ++(EntityId id) => new(id.Value + 1);
}