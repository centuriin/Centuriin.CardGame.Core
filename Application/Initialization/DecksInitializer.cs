using Centuriin.CardGame.Core.Engine;
using Centuriin.CardGame.Core.Engine.Context;
using Centuriin.CardGame.Core.Engine.ECS.Components;
using Centuriin.CardGame.Core.Engine.ECS.Components.Players;
using Centuriin.CardGame.Core.Engine.ECS.Components.Zones;
using Centuriin.CardGame.Core.Engine.ECS.Entities.Factories;
using Centuriin.CardGame.Core.Engine.ECS.Entities.Players;
using Centuriin.CardGame.Core.Engine.ECS.Entities.Zones;
using Centuriin.CardGame.Core.Infrastructure.Storage.Abstractions.Repositories;

namespace Centuriin.CardGame.Core.Application.Initialization;

public sealed class DecksInitializer : IGameEntityInitializer
{
    private readonly IDecksRepository _decksRepository;
    private readonly ICardFactory _cardsFactory;

    public DecksInitializer(
        IDecksRepository decksRepository,
        ICardFactory cardsRepository)
    {
        ArgumentNullException.ThrowIfNull(decksRepository);
        _decksRepository = decksRepository;

        ArgumentNullException.ThrowIfNull(cardsRepository);
        _cardsFactory = cardsRepository;
    }

    public async Task InitializeAsync(
        GameDescription gameDescription,
        IInitialStateContext initialStateContext,
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(gameDescription);
        ArgumentNullException.ThrowIfNull(initialStateContext);

        token.ThrowIfCancellationRequested();

        var deckZones = initialStateContext.Entities
            .Query<Zone>()
            .WithComponent<ZoneRoleComponent>(x => x.Role == ZoneRole.Deck);

        foreach (var zone in deckZones)
        {
            var ownerId = zone.Get<OwnerComponent>().CurrentOwnerId;

            var deckTemplates = await _decksRepository.GetDeckTemplatesAsync(
                gameDescription.GameTypeId,
                initialStateContext.Entities
                    .Get<Player>(ownerId)
                    .Get<PlayerIdentifierComponent>()
                    .PlayerId,
                token);

            var cards = _cardsFactory.Create(deckTemplates);

            foreach (var card in cards)
            {
                card.Add(
                    new ZoneComponent(zone.Id),
                    new OwnerComponent(ownerId));

                initialStateContext.Entities.AddEntity(card);
            }
        }
    }
}
