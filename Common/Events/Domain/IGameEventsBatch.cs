namespace Centuriin.CardGame.Core.Common.Events.Domain;

public interface IGameEventsBatch : IEnumerable<IGameEvent>
{
    public IReadOnlyList<IGameEvent> Events { get; }
}