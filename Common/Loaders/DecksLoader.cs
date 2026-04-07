using Centuriin.CardGame.Core.Common.Components;
using Centuriin.CardGame.Core.Common.Components.Zones;
using Centuriin.CardGame.Core.Common.Entities.Cards;
using Centuriin.CardGame.Core.Common.Entities.Zones;
using Centuriin.CardGame.Core.Common.Factories;
using Centuriin.CardGame.Core.Common.Repositories;

namespace Centuriin.CardGame.Core.Common.Loaders;

public sealed class DecksLoader : IGameLoader
{
    private readonly IDecksRepository _decksRepository;
    private readonly ICardsFactory _cardsFactory;

    public DecksLoader(
        IDecksRepository decksRepository,
        ICardsFactory cardsRepository)
    {
        ArgumentNullException.ThrowIfNull(decksRepository);
        _decksRepository = decksRepository;

        ArgumentNullException.ThrowIfNull(cardsRepository);
        _cardsFactory = cardsRepository;
    }

    public async Task LoadAsync(GameSetup setup, IGameState gameState, CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(setup);
        ArgumentNullException.ThrowIfNull(gameState);

        token.ThrowIfCancellationRequested();

        var deckZones = gameState
            .Query<Zone>()
            .WithComponent<ZoneRoleComponent>(x => x.Role == ZoneRole.Deck)
            .As<Zone>();

        foreach (var zone in deckZones)
        {
            var ownerId = zone.Get<OwnerComponent>().CurrentOwnerId;

            var deckTemplateIds = await _decksRepository.GetDeckTemplateIdsAsync(
                setup.GameTypeId,
                ownerId,
                token);

            var cards = await _cardsFactory.CreateAsync(deckTemplateIds, token);

            foreach (var card in cards)
            {
                card.Add(
                    new ZoneComponent(zone.Id),
                    new OwnerComponent(ownerId));

                gameState.AddEntity<Card, CardId>(card);
            }
        }
    }
}
