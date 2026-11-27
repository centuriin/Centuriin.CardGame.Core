using Centuriin.CardGame.Core.Engine;
using Centuriin.CardGame.Core.Engine.Context;
using Centuriin.CardGame.Core.Engine.ECS.Components;
using Centuriin.CardGame.Core.Engine.ECS.Components.Players;
using Centuriin.CardGame.Core.Engine.ECS.Components.Zones;
using Centuriin.CardGame.Core.Engine.ECS.Entities;
using Centuriin.CardGame.Core.Engine.ECS.Entities.Cards;
using Centuriin.CardGame.Core.Engine.ECS.Entities.Factories;
using Centuriin.CardGame.Core.Engine.ECS.Entities.Players;
using Centuriin.CardGame.Core.Engine.ECS.Entities.Zones;
using Centuriin.CardGame.Core.Engine.ECS.Templates;
using Centuriin.CardGame.Core.Infrastructure.Storage.Abstractions.Repositories;

namespace Centuriin.CardGame.Core.Application.Initialization;

/// <summary>
/// Creates cards from deck templates and places them in their owners' deck zones.
/// </summary>
public sealed class DefaultDecksInitializer : IEntityInitializer
{
    private readonly IDecksRepository _decksRepository;
    private readonly IEntityFactory<Card, CardTemplate> _factory;

    /// <summary>
    /// Initializes a new <see cref="DefaultDecksInitializer"/> instance.
    /// </summary>
    /// <param name="decksRepository">
    /// The repository used to retrieve deck templates.
    /// </param>
    /// <param name="factory">
    /// The factory used to create the required entities or rules.
    /// </param>
    public DefaultDecksInitializer(
        IDecksRepository decksRepository,
        IEntityFactory<Card, CardTemplate> factory)
    {
        ArgumentNullException.ThrowIfNull(decksRepository);
        _decksRepository = decksRepository;

        ArgumentNullException.ThrowIfNull(factory);
        _factory = factory;
    }

    /// <inheritdoc/>
    public async Task<IReadOnlyCollection<EntityBase>> InitializeAsync(
        GameDescription description,
        IReadOnlyInitialStateContext context,
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(description);
        ArgumentNullException.ThrowIfNull(context);

        token.ThrowIfCancellationRequested();

        var result = new List<Card>();

        var deckZones = context.Entities
            .Query<Zone>()
            .WithComponent((ZoneRoleComponent x) => x.Role == ZoneRole.Deck);

        foreach (var zone in deckZones)
        {
            var ownerId = zone.Get<OwnerComponent>().CurrentOwnerId;

            var deckTemplates = await _decksRepository.GetDeckTemplatesAsync(
                description.GameTypeId,
                context.Entities
                    .Get<Player>(ownerId)
                    .Get<PlayerIdentifierComponent>()
                    .PlayerId,
                token);

            var cards = _factory.Create(deckTemplates)
                .Select(x => x.Set(
                    new ZoneComponent(zone.Id),
                    new OwnerComponent(ownerId)))
                .Select(x => x.Entity);

            result.AddRange(cards);
        }

        return result;
    }
}
