namespace Centuriin.CardGame.Core.Engine.Events.Domain;

public interface IGameEvent
{
    public GameId GameId { get; }
}
