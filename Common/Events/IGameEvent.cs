using Centuriin.CardGame.Core.Common.World;

namespace Centuriin.CardGame.Core.Common.Events;

public interface IGameEvent
{
    public GameId GameId { get; }
}
