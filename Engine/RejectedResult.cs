using Centuriin.CardGame.Core.Engine.Root.State;

namespace Centuriin.CardGame.Core.Engine;

/// <summary>
/// Indicates that command validation failed without advancing the game version.
/// </summary>
/// <param name="GameVersion">
/// The version associated with the game operation.
/// </param>
public sealed record class RejectedResult(GameVersion GameVersion) : IEngineResult;