using System.Collections;

using Centuriin.CardGame.Core.Engine.Events.Domain;
using Centuriin.CardGame.Core.Engine.Root.State;

namespace Centuriin.CardGame.Core.Engine.Events;

internal sealed class GameEventsBatch : IGameEventsBatch
{
    public GameVersion CurrentGameVersion { get; }

    public IReadOnlyList<IGameEvent> Events { get; }

    public GameEventsBatch(GameVersion currentGameVersion, IReadOnlyList<IGameEvent> events)
    {
        CurrentGameVersion = currentGameVersion;

        ArgumentNullException.ThrowIfNull(events);
        Events = events;
    }

    public IEnumerator<IGameEvent> GetEnumerator() => Events.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
