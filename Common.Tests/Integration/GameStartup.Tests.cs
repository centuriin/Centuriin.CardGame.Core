using Centuriin.CardGame.Core.Common.Components;
using Centuriin.CardGame.Core.Common.Components.Zones;
using Centuriin.CardGame.Core.Common.Entities.Cards;
using Centuriin.CardGame.Core.Common.Entities.Players;
using Centuriin.CardGame.Core.Common.Entities.Zones;
using Centuriin.CardGame.Core.Common.Events;
using Centuriin.CardGame.Core.Common.Factories;
using Centuriin.CardGame.Core.Common.Loaders;
using Centuriin.CardGame.Core.Common.Logging;
using Centuriin.CardGame.Core.Common.Repositories;
using Centuriin.CardGame.Core.Common.Systems;
using Centuriin.Centuriin.Core.Common;

using FluentAssertions;

using Moq;

using Xunit;

namespace Centuriin.CardGame.Core.Common.SmokeTests;

public sealed class GameStartupIntegrationTests
{
    [Fact]
    [Trait("Category", "Integration")]
    public async Task FullFlowFromLoadersToFirstTurnShouldWorkCorrectlyAsync()
    {
        // Arrange
        var gameId = new GameId(Guid.NewGuid());
        var p1Id = new PlayerId(Guid.NewGuid());
        var gameTypeId = new GameTypeId(1);

        var deckZone = new Zone(new ZoneId(1));
        deckZone.Add(new ZoneRoleComponent(ZoneRole.Deck));

        var handZone = new Zone(new ZoneId(2));
        handZone.Add(new ZoneRoleComponent(ZoneRole.Hand), new HasPrimaryCards(3));

        var zoneTemplateId = new TemplateId(111);
        var zonesRepoMock = new Mock<IZoneDefinitionsRepository>(MockBehavior.Strict);
        zonesRepoMock
            .Setup(x => x.GetZoneDefinitionsAsync(gameTypeId, TestContext.Current.CancellationToken))
            .ReturnsAsync([new ZoneDefinition(zoneTemplateId, ZoneScope.Singleton)]);

        var zonesFactoryMock = new Mock<IZonesFactory>(MockBehavior.Strict);
        zonesFactoryMock.Setup(x => x.CreateAsync(
                It.IsAny<IReadOnlyCollection<TemplateId>>(), 
                TestContext.Current.CancellationToken))
            .ReturnsAsync([deckZone, handZone]);

        var cardTemplateIds = new HashSet<TemplateId> { new(101), new(102), new(103) };
        var decksRepoMock = new Mock<IDecksRepository>(MockBehavior.Strict);
        decksRepoMock.Setup(x => x.GetDeckTemplateIdsAsync(
                gameTypeId, 
                PlayerId.System, 
                TestContext.Current.CancellationToken))
            .ReturnsAsync(cardTemplateIds);

        var cards = cardTemplateIds.Select(id => new Card(new CardId((int)id.Value))).ToList();
        var cardsFactoryMock = new Mock<ICardsFactory>(MockBehavior.Strict);
        cardsFactoryMock.Setup(x => x.CreateAsync(
                cardTemplateIds,
                TestContext.Current.CancellationToken))
            .ReturnsAsync(cards);

        var dispatcher = new EventDispatcher();
        dispatcher.Register<GameStartedEvent>(new SetupTurnFlowSystem(DebugLogger.Instance));
        dispatcher.Register<GameStartedEvent>(new DealerSystem(DebugLogger.Instance));
        dispatcher.Register<TurnFlowDefinedEvent>(new TurnFlowSystem(DebugLogger.Instance));
        dispatcher.Register<CardDealtEvent>(new CardMovementSystem(DebugLogger.Instance));

        var game = new Game(
            new GameState(gameId, new TurnAutomat()),
            Mock.Of<IGameEventsRepository>(),
            dispatcher);

        var setup = new GameSetup(gameTypeId, [p1Id]);

        var gameFactoryMock = new Mock<IGameFactory>(MockBehavior.Strict);
        gameFactoryMock.Setup(x => x.Create(setup)).Returns(game);

        var loaders = new List<IGameLoader>
        {
            new ClassicPlayersLoader(),
            new ZonesLoader(zonesRepoMock.Object, zonesFactoryMock.Object),
            new DecksLoader(decksRepoMock.Object, cardsFactoryMock.Object)
        };

        var startupService = new GameStartupService(loaders, gameFactoryMock.Object);

        // Act
        var resultGame = await startupService.StartupGameAsync(
            setup,
            TestContext.Current.CancellationToken);

        // Assert
        resultGame.Should().BeSameAs(game);

        var cardsInHand = game.State.Query<Card>()
            .WithComponent<ZoneComponent>(z => z.CurrentZoneId == handZone.Id)
            .ToList();

        cardsInHand.Should().HaveCount(3);
        cardsInHand.All(c => c.Get<OwnerComponent>().CurrentOwnerId == p1Id).Should().BeTrue();

        var cardsInDeck = game.State.Query<Card>()
            .WithComponent<ZoneComponent>(z => z.CurrentZoneId == deckZone.Id)
            .ToList();
        cardsInDeck.Should().BeEmpty();

        game.State.TurnAutomat.ActivePlayer.Should().Be(p1Id);
    }
}