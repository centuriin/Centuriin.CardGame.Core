using Centuriin.CardGame.Core.Common.Commands;
using Centuriin.CardGame.Core.Common.Events.Application;
using Centuriin.CardGame.Core.Common.Events.Domain;
using Centuriin.CardGame.Core.Common.Root.State;

namespace Centuriin.CardGame.Core.Common.Root;

public interface IGame
{
    public GameId Id { get; }

    public GameVersion Version { get; }

    public IReadOnlyGameState State { get; }

    public void Apply(IGameEventsBatch batch);

    public IGameApplicationEvent Start();

    public IGameApplicationEvent Execute(ICommand command);
}
