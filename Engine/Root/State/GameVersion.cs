namespace Centuriin.CardGame.Core.Engine.Root.State;

public readonly record struct GameVersion : IComparable<GameVersion>
{
    public static GameVersion Default { get; } = new(0);

    public int Value { get; }

    public GameVersion(int value)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(value);
        Value = value;
    }

    public int CompareTo(GameVersion other) => Value.CompareTo(other);

    public static GameVersion operator ++(GameVersion version) => new(version.Value + 1);
}