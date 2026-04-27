namespace Centuriin.CardGame.Core.Common.Entities;

/// <summary>
/// Entity id.
/// </summary>
/// <param name="Value">
/// Value.
/// </param>
public readonly record struct EntityId(int Value)
{
    public static EntityId Default { get; }
}