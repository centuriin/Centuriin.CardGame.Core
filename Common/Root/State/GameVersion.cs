namespace Centuriin.CardGame.Core.Common.Root.State;

public readonly record struct GameVersion
{
    public static GameVersion Default { get; } = new(0);

    public int Value { get; }

    public GameVersion(int value)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(value);
        Value = value;
    }

    public static GameVersion operator ++(GameVersion version) => new(version.Value + 1);
}