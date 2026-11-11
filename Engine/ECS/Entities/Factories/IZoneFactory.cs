using Centuriin.CardGame.Core.Engine.ECS.Entities.Zones;
using Centuriin.CardGame.Core.Engine.ECS.Templates;

namespace Centuriin.CardGame.Core.Engine.ECS.Entities.Factories;

public interface IZoneFactory
{
    public IReadOnlyCollection<Zone> Create(
        IReadOnlyCollection<ZoneTemplate> templates,
        int activePlayersCount);
}
