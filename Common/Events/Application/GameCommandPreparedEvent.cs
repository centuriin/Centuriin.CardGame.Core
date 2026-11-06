using Centuriin.CardGame.Core.Common.Commands;
using Centuriin.CardGame.Core.Common.Root.State;

namespace Centuriin.CardGame.Core.Common.Events.Application;

public sealed class GameCommandPreparedEvent : IGameApplicationEvent
{
    public GameId GameId { get; }

    public GameVersion GameVersion { get; }

    public ICommand Command { get; }

    public GameCommandPreparedEvent(
        GameId gameId,
        GameVersion gameVersion,
        ICommand command)
    {
        GameId = gameId;
        GameVersion = gameVersion;

        ArgumentNullException.ThrowIfNull(command);
        Command = command;
    }
}
