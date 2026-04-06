using Centuriin.CardGame.Core.Common.Components;
using Centuriin.CardGame.Core.Common.Components.Zones;
using Centuriin.CardGame.Core.Common.Entities.Cards;
using Centuriin.CardGame.Core.Common.Entities.Players;
using Centuriin.CardGame.Core.Common.Entities.Zones;
using Centuriin.CardGame.Core.Common.Factories;
using Centuriin.CardGame.Core.Common.Repositories;

using FluentAssertions;

using Moq;

using Xunit;

namespace Centuriin.CardGame.Core.Common.Loaders;

public sealed class DecksLoaderTests
{
    [Fact]
    public async Task LoadAsyncShouldFetchDeckCompositionAndAddCardsToGameStateAsync()
    {
        // Arrange
        var gameTypeId = new GameTypeId(1);
        var ownerId = new PlayerId(Guid.NewGuid());

        var deckZone = new Zone(new(10));
        deckZone.Add(
            [
                new ZoneRoleComponent(ZoneRole.Deck),
                new OwnerComponent(ownerId)
            ]);

        var addedCards = new List<Card>();
        var gameStateMock = new Mock<IGameState>(MockBehavior.Strict);
        gameStateMock
            .Setup(x => x.Query<Zone>())
            .Returns([deckZone]);
        gameStateMock
            .Setup(x => x.AddEntity<Card, CardId>(It.IsAny<Card>()))
            .Callback<Card>(addedCards.Add);

        var templateIds = new HashSet<TemplateId> { new(101), new(102) };
        var decksRepoMock = new Mock<IDecksRepository>(MockBehavior.Strict);
        decksRepoMock
            .Setup(x => x.GetDeckTemplateIdsAsync(gameTypeId, ownerId, TestContext.Current.CancellationToken))
            .ReturnsAsync(templateIds);

        var card1 = new Card(new CardId(1));
        var card2 = new Card(new CardId(2));
        var createdCards = new List<Card> { card1, card2 };
        var cardsFactoryMock = new Mock<ICardsFactory>(MockBehavior.Strict);
        cardsFactoryMock
            .Setup(x => x.CreateAsync(templateIds, TestContext.Current.CancellationToken))
            .ReturnsAsync(createdCards);

        var loader = new DecksLoader(
            gameStateMock.Object,
            decksRepoMock.Object,
            cardsFactoryMock.Object);

        // Act
        await loader.LoadAsync(gameTypeId, TestContext.Current.CancellationToken);

        // Assert
        addedCards.Should().HaveCount(2);

        addedCards.All(c => c.Get<ZoneComponent>().CurrentZoneId == deckZone.Id)
            .Should().BeTrue();

        addedCards.Should().Contain(card1);
        addedCards.Should().Contain(card2);
    }

    [Fact]
    public async Task LoadAsyncShouldDoNothingWhenNoDeckZonesFoundAsync()
    {
        // Arrange
        var gameTypeId = new GameTypeId(2);

        var addedCards = new List<Card>();
        var gameStateMock = new Mock<IGameState>(MockBehavior.Strict);
        gameStateMock
            .Setup(x => x.Query<Zone>())
            .Returns([]);
        gameStateMock
            .Setup(x => x.AddEntity<Card, CardId>(It.IsAny<Card>()))
            .Callback<Card>(addedCards.Add);

        var loader = new DecksLoader(
            gameStateMock.Object,
            Mock.Of<IDecksRepository>(MockBehavior.Strict),
            Mock.Of<ICardsFactory>(MockBehavior.Strict));

        // Act
        await loader.LoadAsync(gameTypeId, TestContext.Current.CancellationToken);

        // Assert
        addedCards.Should().BeEmpty();
    }
}
