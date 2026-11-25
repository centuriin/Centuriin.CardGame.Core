namespace Centuriin.CardGame.Core.Engine.Root.State;

/// <summary>
/// Identifies the event-batch version of a game.
/// </summary>
public readonly record struct GameVersion : IComparable<GameVersion>
{
    /// <summary>
    /// Gets the version before any event batches have been executed.
    /// </summary>
    public static GameVersion Default { get; } = new(0);

    /// <summary>
    /// Gets the underlying numeric value.
    /// </summary>
    public int Value { get; }

    /// <summary>
    /// Initializes a new <see cref="GameVersion"/> instance.
    /// </summary>
    /// <param name="value">
    /// The non-negative numeric version.
    /// </param>
    public GameVersion(int value)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(value);
        Value = value;
    }

    /// <inheritdoc/>
    public int CompareTo(GameVersion other) => Value.CompareTo(other.Value);

    /// <summary>
    /// Returns the next sequential value.
    /// </summary>
    /// <param name="version">
    /// The game version to advance.
    /// </param>
    /// <returns>
    /// The resulting value.
    /// </returns>
    public static GameVersion operator ++(GameVersion version) => 
        new(version.Value + 1);

    /// <summary>
    /// Determines whether the first version is greater than the second.
    /// </summary>
    /// <param name="first">
    /// The first value to compare.
    /// </param>
    /// <param name="second">
    /// The second value to compare.
    /// </param>
    /// <returns>
    /// The result of the comparison.
    /// </returns>
    public static bool operator >(GameVersion first, GameVersion second) => 
        first.Value > second.Value;

    /// <summary>
    /// Determines whether the first version is greater than or equal to the second.
    /// </summary>
    /// <param name="first">
    /// The first value to compare.
    /// </param>
    /// <param name="second">
    /// The second value to compare.
    /// </param>
    /// <returns>
    /// The result of the comparison.
    /// </returns>
    public static bool operator >=(GameVersion first, GameVersion second) =>
        first.Value >= second.Value;

    /// <summary>
    /// Determines whether the first version is less than the second.
    /// </summary>
    /// <param name="first">
    /// The first value to compare.
    /// </param>
    /// <param name="second">
    /// The second value to compare.
    /// </param>
    /// <returns>
    /// The result of the comparison.
    /// </returns>
    public static bool operator <(GameVersion first, GameVersion second) => 
        first.Value < second.Value;

    /// <summary>
    /// Determines whether the first version is less than or equal to the second.
    /// </summary>
    /// <param name="first">
    /// The first value to compare.
    /// </param>
    /// <param name="second">
    /// The second value to compare.
    /// </param>
    /// <returns>
    /// The result of the comparison.
    /// </returns>
    public static bool operator <=(GameVersion first, GameVersion second) =>
        first.Value <= second.Value;
}
