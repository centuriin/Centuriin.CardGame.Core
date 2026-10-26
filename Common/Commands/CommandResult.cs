using Centuriin.CardGame.Core.Common.World;

namespace Centuriin.CardGame.Core.Common.Commands;

public sealed record CommandResult(bool IsSuccess, GameVersion Version);
