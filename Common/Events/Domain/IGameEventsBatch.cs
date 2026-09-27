namespace Centuriin.CardGame.Core.Common.Events.Domain;

public interface IGameEventsBatch
{
    public IReadOnlyCollection<IGameEvent> Events { get; }
}