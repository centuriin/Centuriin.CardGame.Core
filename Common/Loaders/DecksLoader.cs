using Centuriin.CardGame.Core.Common.Components;
using Centuriin.CardGame.Core.Common.Entities.Cards;
using Centuriin.CardGame.Core.Common.Entities.Zones;
using Centuriin.CardGame.Core.Common.Factories;
using Centuriin.CardGame.Core.Common.Repositories;

namespace Centuriin.CardGame.Core.Common.Loaders;

public sealed class DecksLoader : IDecksLoader
{
    private readonly IGameState _gameState;
    private readonly IDecksRepository _decksRepository;
    private readonly ICardsFactory _cardsFactory;

    public DecksLoader(
        IGameState gameState,
        IDecksRepository decksRepository,
        ICardsFactory cardsRepository)
    {
        ArgumentNullException.ThrowIfNull(gameState);
        _gameState = gameState;

        ArgumentNullException.ThrowIfNull(decksRepository);
        _decksRepository = decksRepository;

        ArgumentNullException.ThrowIfNull(cardsRepository);
        _cardsFactory = cardsRepository;
    }

    public async Task LoadAsync(GameTypeId gameTypeId, CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        var deckZones = _gameState
            .Query<Zone>()
            .WithComponent<ZoneRoleComponent>(x => x.Role == ZoneRole.Deck)
            .As<Zone>();

        foreach (var zone in deckZones)
        {
            var ownerId = zone.Get<OwnerComponent>().CurrentOwnerId;

            var deckTemplateIds = await _decksRepository.GetDeckTemplateIdsAsync(
                gameTypeId,
                ownerId,
                token);

            var cards = await _cardsFactory.CreateAsync(deckTemplateIds, token);

            foreach (var card in cards)
            {
                card.Add(new ZoneComponent(zone.Id));

                _gameState.AddEntity<Card, CardId>(card);
            }
        }
    }
}
