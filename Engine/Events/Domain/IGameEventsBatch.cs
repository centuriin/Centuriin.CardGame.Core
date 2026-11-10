using Centuriin.CardGame.Core.Engine.Root.State;

namespace Centuriin.CardGame.Core.Engine.Events.Domain;

public interface IGameEventsBatch : IEnumerable<IGameEvent>
{
    public GameVersion GameVersion { get; }

    public IReadOnlyList<IGameEvent> Events { get; }
}