using System.Collections.Frozen;

using Centuriin.CardGame.Core.Engine;
using Centuriin.CardGame.Core.Engine.ECS.Components.Zones;
using Centuriin.CardGame.Core.Engine.ECS.Entities.Zones;
using Centuriin.CardGame.Core.Engine.ECS.Templates;

namespace Centuriin.CardGame.Core.Infrastructure.Storage.Abstractions.Repositories.InMemory;

public sealed class ZoneTemplatesRepository : ITemplatesRepository<ZoneTemplate>
{
    // For drunkard
    private static FrozenDictionary<TemplateId, ZoneTemplate> DrunkardZones { get; } =
        new Dictionary<TemplateId, ZoneTemplate>()
        {
                {
                    new(1),
                    new ZoneTemplate(
                        new(1),
                        [
                            new ZoneRoleComponent(ZoneRole.Deck),
                        ],
                        ZoneScope.Singleton)
                },
                {
                    new(2),
                    new ZoneTemplate(
                        new(2),
                        [
                            new ZoneRoleComponent(ZoneRole.Hand),
                            new HasPrimaryCards(3)
                        ],
                        ZoneScope.PerPlayer)
                },
                {
                    new(4),
                    new ZoneTemplate(
                        new(4),
                        [
                            new ZoneRoleComponent(ZoneRole.Slot),
                        ],
                        ZoneScope.PerPlayer)
                },
        }
        .ToFrozenDictionary();

    public Task<ZoneTemplate> GetByIdAsync(TemplateId templateId, CancellationToken token) =>
        Task.FromResult(DrunkardZones[templateId]);

    public Task<IReadOnlyCollection<ZoneTemplate>> GetTemplatesByIdsAsync(
        IReadOnlyCollection<TemplateId> templateIds,
        CancellationToken token)
        => Task.FromResult<IReadOnlyCollection<ZoneTemplate>>(DrunkardZones.Values);
}
