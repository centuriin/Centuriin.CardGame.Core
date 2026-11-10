using Centuriin.CardGame.Core.Engine.Root.State;

namespace Centuriin.CardGame.Core.Engine.Commands;

public sealed record CommandResult(bool IsSuccess, GameVersion Version);
