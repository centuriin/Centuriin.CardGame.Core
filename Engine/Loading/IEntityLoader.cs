using Centuriin.CardGame.Core.Engine.Root.State;

namespace Centuriin.CardGame.Core.Engine.Loading;

public interface IEntityLoader
{
    public void Load(
        IReadOnlyEntitiesCollection entitiesCollection,
        IGameState gameState);
}