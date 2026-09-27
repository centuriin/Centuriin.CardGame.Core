namespace Centuriin.CardGame.Core.Common.Events.Domain;

public interface IGameEventApplier
{
    public IGameEventsBatch Apply(IGameEvent @event);
}