namespace Centuriin.CardGame.Core.Common.Events;

public interface IGameEventUnit
{
    public IPrimaryEvent PrimaryEvent { get; }

    public IReadOnlyCollection<IRandomEvent> RelatedRandomEvents { get; }
}
