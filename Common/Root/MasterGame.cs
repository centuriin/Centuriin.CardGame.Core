using System.Collections;

using Centuriin.CardGame.Core.Common.Events.Application;
using Centuriin.CardGame.Core.Common.Events.Dispatching;
using Centuriin.CardGame.Core.Common.Events.Domain;
using Centuriin.CardGame.Core.Common.Root.State;

namespace Centuriin.CardGame.Core.Common.Root;

public sealed class MasterGame : GameBase
{
    public MasterGame(
        GameId id,
        GameTypeId typeId,
        GameStatus status,
        IGameState state,
        IGameEventApplier eventApplier,
        IAsyncDispatcher<IGameApplicationEvent> dispatcher)
        : base(id, typeId, status, state, eventApplier, dispatcher)
    {
    }

    protected override Task<IGameEventsBatch> StartCoreAsync(
        CancellationToken token)
    {
        var appliedEvents = EventApplier.Apply(new GameStartedEvent(Id));

        return Task.FromResult<IGameEventsBatch>(
            new EventsBatch(IncrementVersion(), appliedEvents));
    }

    protected override Task<IGameEventsBatch> ApplyCoreAsync(
        IGameEvent @event,
        CancellationToken token)
    {
        var appliedEvents = EventApplier.Apply(@event);

        return Task.FromResult<IGameEventsBatch>(
            new EventsBatch(IncrementVersion(), appliedEvents));
    }

    protected override Task<IGameEventsBatch> AbortCoreAsync(CancellationToken token)
        => Task.FromResult<IGameEventsBatch>(new EventsBatch(IncrementVersion(), []));

    private GameVersion IncrementVersion() => new(Version.Value + 1);

    private sealed class EventsBatch : IGameEventsBatch
    {
        public GameVersion CurrentVersion { get; }

        public IReadOnlyList<IGameEvent> Events { get; }

        public EventsBatch(GameVersion currentVersion, IReadOnlyList<IGameEvent> events)
        {
            CurrentVersion = currentVersion;

            ArgumentNullException.ThrowIfNull(events);
            Events = events;
        }

        public IEnumerator<IGameEvent> GetEnumerator() => Events.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
