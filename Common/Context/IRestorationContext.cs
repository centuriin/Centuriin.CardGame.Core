using Centuriin.CardGame.Core.Common.Events.Domain;
using Centuriin.CardGame.Core.Common.World;

namespace Centuriin.CardGame.Core.Common.Context;

public interface IRestorationContext
{
    public IGameEventsBatch RestorationBatch { get; }

    public GameVersion GameVersion { get; }
}