using Centuriin.CardGame.Core.Common.Components;
using Centuriin.CardGame.Core.Common.Entities.Cards;
using Centuriin.CardGame.Core.Common.Entities.Zones;
using Centuriin.CardGame.Core.Common.Repositories;

namespace Centuriin.CardGame.Core.Common;

public sealed class DecksLoader : IDecksLoader
{
    private readonly IGameState _gameState;
    private readonly ICardTemplatesRepository _templatesRepository;

    public DecksLoader(
        IGameState gameState,
        ICardTemplatesRepository templatesRepository)
    {
        ArgumentNullException.ThrowIfNull(gameState);
        _gameState = gameState;

        ArgumentNullException.ThrowIfNull(templatesRepository);
        _templatesRepository = templatesRepository;
    }

    public async Task LoadAsync(GameTypeId gameTypeId, CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        var templates = await _templatesRepository
            .GetTemplatesByGameTypeAsync(gameTypeId, token);

        var zoneId = _gameState.Query<Zone, ZoneId>()
            .Where(x =>
                x.Get<ZoneRoleComponent>().Role == ZoneRole.Deck
                && x.IsEmpty);

        var index = 0;
        foreach (var template in templates)
        {
            var card = CreateCard(template, ++index);

            _gameState.AddEntity<Card, CardId>(card);
        }
    }

    private static Card CreateCard(CardTemplate template, int cardId)
    {
        var card = new Card(new(cardId));

        foreach (var component in template.Components)
        {
            card.Add(component.Copy());
        }

        card.Add(new TemplateComponent(template.Id));

        card.Add(new ZoneComponent())

        return card;
    }
}
