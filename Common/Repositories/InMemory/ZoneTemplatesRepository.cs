using System.Collections.Frozen;

using Centuriin.CardGame.Core.Common.Components.Zones;
using Centuriin.CardGame.Core.Common.Entities.Zones;

namespace Centuriin.CardGame.Core.Common.Repositories.InMemory;

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
                        ])
                },
                {
                    new(2),
                    new ZoneTemplate(
                        new(2),
                        [
                            new ZoneRoleComponent(ZoneRole.Hand),
                        ])
                },
                {
                    new(4),
                    new ZoneTemplate(
                        new(4),
                        [
                            new ZoneRoleComponent(ZoneRole.Slot),
                        ])
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
