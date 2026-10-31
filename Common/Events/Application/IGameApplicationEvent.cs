using Centuriin.CardGame.Core.Common.Root.State;

namespace Centuriin.CardGame.Core.Common.Events.Application;

public interface IGameApplicationEvent
{
    public GameId GameId { get; }

    public GameVersion GameVersion { get; }
}