using Centuriin.CardGame.Core.Common.Events.Domain;
using Centuriin.CardGame.Core.Common.World;

namespace Centuriin.CardGame.Core.Common.Events.Application;

public sealed class NewGameEventsBatchCreatedEvent : IApplicationEvent
{
    public GameId GameId { get; }

    public IReadOnlyCollection<IGameEvent> GameEvents { get; }

    public NewGameEventsBatchCreatedEvent(
        GameId gameId,
        IReadOnlyCollection<IGameEvent> gameEvents)
    {
        GameId = gameId;

        ArgumentNullException.ThrowIfNull(gameEvents);
        GameEvents = gameEvents;
    }
}
