namespace Centuriin.CardGame.Core.Common.Events.Domain;

public interface IGameEventApplier
{
    public IReadOnlyList<IGameEvent> Apply(IGameEvent @event);

    public void SilentApply(IReadOnlyList<IGameEvent> batch);
}