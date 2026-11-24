using Centuriin.CardGame.Core.Engine;
using Centuriin.CardGame.Core.Engine.Commands;
using Centuriin.CardGame.Core.Engine.Events.Domain;

namespace Centuriin.CardGame.Core.Drunkard;

public sealed record class FakeCommand(GameId GameId, PlayerId Actor) : ICommand
{
    public IGameEvent YieldGameEvent() => new GameStartedEvent(GameId);
}