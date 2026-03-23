using Centuriin.Centuriin.Core.Common;

namespace Centuriin.CardGame.Core.Common.Events.Game;

public interface IGameEvent
{
    public GameId GameId { get; }
}
