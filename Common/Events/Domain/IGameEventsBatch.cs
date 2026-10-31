using Centuriin.CardGame.Core.Common.Root.State;

namespace Centuriin.CardGame.Core.Common.Events.Domain;

public interface IGameEventsBatch : IEnumerable<IGameEvent>
{
    public GameVersion CurrentGameVersion { get; }

    public IReadOnlyList<IGameEvent> Events { get; }
}