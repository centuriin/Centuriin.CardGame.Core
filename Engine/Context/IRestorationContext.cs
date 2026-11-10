using Centuriin.CardGame.Core.Engine.Events.Domain;

namespace Centuriin.CardGame.Core.Engine.Context;

public interface IRestorationContext : IGameContext
{
    public IGameEventsBatch RestorationBatch { get; }
}