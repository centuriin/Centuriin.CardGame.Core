using Centuriin.CardGame.Core.Common.World;

namespace Centuriin.CardGame.Core.Common.Events.Domain;

public interface IGameEvent
{
    public GameId GameId { get; }
}
