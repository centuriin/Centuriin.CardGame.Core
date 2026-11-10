using Centuriin.CardGame.Core.Engine.Commands;
using Centuriin.CardGame.Core.Engine.Events.Domain;
using Centuriin.CardGame.Core.Engine.Root.State;

namespace Centuriin.CardGame.Core.Engine.Root;

public interface IGame
{
    public GameId Id { get; }

    public GameVersion Version { get; }

    public IReadOnlyGameState State { get; }

    public void Apply(IGameEventsBatch batch);

    public IEngineResult Start();

    public IEngineResult Execute(ICommand command);
}
