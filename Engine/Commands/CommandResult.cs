using Centuriin.CardGame.Core.Engine.Root.State;

namespace Centuriin.CardGame.Core.Engine.Commands;

/// <summary>
/// Describes command success and the associated game version.
/// </summary>
/// <param name="IsSuccess">
/// Whether the command succeeded.
/// </param>
/// <param name="Version">
/// The game version associated with the command result.
/// </param>
public sealed record CommandResult(bool IsSuccess, GameVersion Version);
