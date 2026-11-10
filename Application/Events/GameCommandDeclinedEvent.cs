using Centuriin.CardGame.Core.Engine;
using Centuriin.CardGame.Core.Engine.Commands;
using Centuriin.CardGame.Core.Engine.Root.State;

namespace Application.Events;

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