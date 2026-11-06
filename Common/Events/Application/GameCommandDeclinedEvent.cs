using Centuriin.CardGame.Core.Common.Commands;
using Centuriin.CardGame.Core.Common.Root.State;

namespace Centuriin.CardGame.Core.Common.Events.Application;

public sealed record class GameCommandDeclinedEvent : IGameApplicationEvent
{
    public GameId GameId { get; }

    public GameVersion GameVersion { get; }

    public ICommand Command { get; }

    public GameCommandDeclinedEvent(
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