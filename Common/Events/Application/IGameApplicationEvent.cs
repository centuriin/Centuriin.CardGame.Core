using Centuriin.CardGame.Core.Common.World;

namespace Centuriin.CardGame.Core.Common.Events.Application;

public interface IGameApplicationEvent
{
    public GameId GameId { get; }

    public GameVersion GameVersion { get; }
}