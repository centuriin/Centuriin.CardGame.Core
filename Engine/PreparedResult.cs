using Centuriin.CardGame.Core.Engine.Root.State;

namespace Centuriin.CardGame.Core.Engine;

/// <summary>
/// Indicates that a replica operation is prepared for execution by an external authority.
/// </summary>
/// <param name="GameVersion">
/// The version associated with the game operation.
/// </param>
public sealed record class PreparedResult(GameVersion GameVersion) : IEngineResult;