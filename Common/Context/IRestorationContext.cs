using Centuriin.CardGame.Core.Common.Events.Domain;

namespace Centuriin.CardGame.Core.Common.Context;

public interface IRestorationContext : IGameContext
{
    public IGameEventsBatch RestorationBatch { get; }
}