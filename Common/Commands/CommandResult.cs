using Centuriin.CardGame.Core.Common.Root.State;

namespace Centuriin.CardGame.Core.Common.Commands;

public sealed record CommandResult(bool IsSuccess, GameVersion Version);
