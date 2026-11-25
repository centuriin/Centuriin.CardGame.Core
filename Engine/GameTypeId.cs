namespace Centuriin.CardGame.Core.Engine;

/// <summary>
/// Identifies a registered kind of game.
/// </summary>
/// <param name="Value">
/// The underlying identifier value.
/// </param>
public readonly record struct GameTypeId(int Value);