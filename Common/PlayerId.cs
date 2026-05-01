namespace Centuriin.CardGame.Core.Common;

/// <summary>
/// External global player identifier.
/// </summary>
/// <param name="Value">
/// Value.
/// </param>
public readonly record struct PlayerId(Guid Value)
{
    /// <summary>
    /// System player identifier.
    /// </summary>
    public static PlayerId System { get; } =
        new(Guid.Parse("00000000-0000-7000-8000-000000000000"));
}