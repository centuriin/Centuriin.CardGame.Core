using Centuriin.CardGame.Core.Engine.Events.Domain;

namespace Centuriin.CardGame.Core.Engine.Commands;

public interface ICommand
{
    public GameId GameId { get; }

    public PlayerId Actor { get; }

    public IGameEvent YieldGameEvent();
}
