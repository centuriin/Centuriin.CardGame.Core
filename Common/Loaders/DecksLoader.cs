using Centuriin.CardGame.Core.Common.Components;
using Centuriin.CardGame.Core.Common.Components.Players;
using Centuriin.CardGame.Core.Common.Components.Zones;
using Centuriin.CardGame.Core.Common.Entities.Players;
using Centuriin.CardGame.Core.Common.Entities.Zones;
using Centuriin.CardGame.Core.Common.Factories;
using Centuriin.CardGame.Core.Common.Repositories;
using Centuriin.CardGame.Core.Common.World;

namespace Centuriin.CardGame.Core.Common.Loaders;

public sealed class DecksLoader : IGameLoader
{
    private readonly IDecksRepository _decksRepository;
    private readonly ICardFactory _cardsFactory;

    public DecksLoader(
        IDecksRepository decksRepository,
        ICardFactory cardsRepository)
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
                gameState.Get<Player>(ownerId).Get<PlayerIdentifierComponent>().PlayerId,
                token);

            var cards = await _cardsFactory.CreateAsync(deckTemplateIds, token);

            foreach (var card in cards)
            {
                card.Add(
                    new ZoneComponent(zone.Id),
                    new OwnerComponent(ownerId));

                gameState.AddEntity(card);
            }
        }
    }
}
