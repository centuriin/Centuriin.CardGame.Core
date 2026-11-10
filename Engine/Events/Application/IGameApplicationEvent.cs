using Centuriin.CardGame.Core.Engine.Root.State;

namespace Centuriin.CardGame.Core.Engine.Events.Application;

public interface IGameApplicationEvent
{
    public GameId GameId { get; }

    public GameVersion GameVersion { get; }
}