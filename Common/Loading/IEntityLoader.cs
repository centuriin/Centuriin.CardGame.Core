using Centuriin.CardGame.Core.Common.Root.State;

namespace Centuriin.CardGame.Core.Common.Loading;

public interface IEntityLoader
{
    public void Load(
        IReadOnlyEntitiesCollection entitiesCollection, 
        IGameState gameState);
}