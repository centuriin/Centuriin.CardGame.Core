using Centuriin.CardGame.Core.Application.Initialization;
using Centuriin.CardGame.Core.Engine;
using Centuriin.CardGame.Core.Engine.Context;
using Centuriin.CardGame.Core.Engine.ECS.Components;
using Centuriin.CardGame.Core.Engine.ECS.Components.Cards;
using Centuriin.CardGame.Core.Engine.ECS.Components.Players;
using Centuriin.CardGame.Core.Engine.ECS.Components.Zones;
using Centuriin.CardGame.Core.Engine.ECS.Entities;
using Centuriin.CardGame.Core.Engine.ECS.Entities.Cards;
using Centuriin.CardGame.Core.Engine.ECS.Entities.Factories;
using Centuriin.CardGame.Core.Engine.ECS.Entities.Players;
using Centuriin.CardGame.Core.Engine.ECS.Entities.Zones;
using Centuriin.CardGame.Core.Engine.ECS.Templates;
using Centuriin.CardGame.Core.Infrastructure.Storage.Abstractions.Repositories;

namespace Centuriin.CardGame.Core.Drunkard.Initialization;

/// <summary>
/// Creates cards matching the selected deck size and assigns their deck zones and owners.
/// </summary>
internal sealed class DeckInitializer : IEntityInitializer
{
    private readonly IDecksRepository _decksRepository;
    private readonly IEntityFactory<Card, CardTemplate> _factory;

    /// <summary>
    /// Initializes a new <see cref="DeckInitializer"/> instance.
    /// </summary>
    /// <param name="decksRepository">
    /// The repository supplying the full 52-card deck templates.
    /// </param>
    /// <param name="factory">
    /// The factory used to create the selected cards.
    /// </param>
    public DeckInitializer(
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

        if (description.GameSettings is not GameSettings settings)
        {
            throw new ArgumentException(
                "Drunkard game settings are required.",
                nameof(description));
        }

        var minimumRank = settings.DeckType switch
        {
            DeckType.Deck36 => CardRank.Six,
            DeckType.Deck52 => CardRank.Two,
            _ => throw new InvalidOperationException(
                $"Unsupported deck type {settings.DeckType}.")
        };

        var deckZone = context.Entities
            .Query<Zone>()
            .WithComponent((ZoneRoleComponent x) => x.Role == ZoneRole.Deck)
            .Single();

        var ownerId = deckZone.Get<OwnerComponent>().CurrentOwnerId;
        var playerId = context.Entities
            .Get<Player>(ownerId)
            .Get<PlayerIdentifierComponent>()
            .PlayerId;

        var deckTemplates = await _decksRepository.GetDeckTemplatesAsync(
            description.GameTypeId,
            playerId,
            token);

        var selectedTemplates = deckTemplates
            .Where(template => template.Components.OfType<RankComponent>().Single().Rank >= minimumRank)
            .ToArray();

        var cards = _factory.Create(selectedTemplates)
            .Select(x => x.Set(
                new ZoneComponent(deckZone.Id),
                new OwnerComponent(ownerId)))
            .Select(x => x.Entity);

        return [.. cards];
    }
}
