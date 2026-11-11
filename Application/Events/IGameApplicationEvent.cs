using Centuriin.CardGame.Core.Engine;
using Centuriin.CardGame.Core.Engine.Root.State;

namespace Centuriin.CardGame.Core.Application.Events;

public interface IGameApplicationEvent
{
    public GameId GameId { get; }

    public GameVersion GameVersion { get; }
}