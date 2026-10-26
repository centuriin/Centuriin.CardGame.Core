namespace Centuriin.CardGame.Core.Common.Events.Domain;

public interface IGameEventsBatch
{
    public IReadOnlyList<IGameEvent> Events { get; }
}