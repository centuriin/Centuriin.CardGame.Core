using Centuriin.CardGame.Core.Common.Commands;
using Centuriin.CardGame.Core.Common.Components;
using Centuriin.CardGame.Core.Common.Components.Zones;
using Centuriin.CardGame.Core.Common.Entities;
using Centuriin.CardGame.Core.Common.Entities.Cards;
using Centuriin.CardGame.Core.Common.Entities.Zones;
using Centuriin.CardGame.Core.Common.Events;
using Centuriin.CardGame.Core.Common.Events.Dispatching;
using Centuriin.CardGame.Core.Common.Factories;
using Centuriin.CardGame.Core.Common.Loaders;
using Centuriin.CardGame.Core.Common.Observability.Logging;
using Centuriin.CardGame.Core.Common.Repositories;
using Centuriin.CardGame.Core.Common.Systems;
using Centuriin.CardGame.Core.Common.Templates;
using Centuriin.CardGame.Core.Common.World;

using FluentAssertions;

using Moq;

using Xunit;

namespace Centuriin.CardGame.Core.Common.Tests.Integration;

public sealed class GameStartupIntegrationTests
{
    [Fact]
    [Trait("Category", "Integration")]
    public async Task FullFlowFromLoadersToFirstTurnShouldWorkCorrectlyAsync()
    {
        // Arrange
        var gameId = new GameId(Guid.NewGuid());
        var p1Id = new EntityId(1);
        var gameTypeId = new GameTypeId(1);

        var deckZone = new Zone(new(1));
        deckZone.Add(new ZoneRoleComponent(ZoneRole.Deck));

        var handZone = new Zone(new(2));
        handZone.Add(new ZoneRoleComponent(ZoneRole.Hand), new HasPrimaryCards(3));

        var zoneTemplateId = new TemplateId(111);
        var zonesRepoMock = new Mock<IZoneDefinitionsRepository>(MockBehavior.Strict);
        zonesRepoMock
            .Setup(x => x.GetZoneDefinitionsAsync(gameTypeId, TestContext.Current.CancellationToken))
            .ReturnsAsync([new ZoneDefinition(zoneTemplateId, ZoneScope.Singleton)]);

        var zonesFactoryMock = new Mock<IZoneFactory>(MockBehavior.Strict);
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

        var cards = cardTemplateIds.Select(id => new Card(new((int)id.Value))).ToList();
        var cardsFactoryMock = new Mock<ICardFactory>(MockBehavior.Strict);
        cardsFactoryMock.Setup(x => x.CreateAsync(
                cardTemplateIds,
                TestContext.Current.CancellationToken))
            .ReturnsAsync(cards);

        var dispatcher = new EventDispatcher();
        dispatcher.Register<GameStartedEvent>(new SetupTurnFlowSystem(
            DebugLogger<SetupTurnFlowSystem>.Instance));
        dispatcher.Register<GameStartedEvent>(new DealerSystem(
            DebugLogger<DealerSystem>.Instance));
        dispatcher.Register<TurnFlowDefinedEvent>(
            new TurnFlowSystem(DebugLogger<TurnFlowSystem>.Instance));
        dispatcher.Register<CardDealtEvent>(new CardMovementSystem(
            DebugLogger<CardMovementSystem>.Instance));

        var game = new Game(
            gameId,
            new GameState(new TurnAutomat()),
            Mock.Of<ICommandValidator>(),
            Mock.Of<IGameEventsRepository>(),
            dispatcher);

        var setup = new GameSetup(gameTypeId, [new PlayerId(Guid.NewGuid())]);

        var sessionMock = new Mock<IGameSession>(MockBehavior.Strict);
        sessionMock.SetupGet(x => x.Game).Returns(game);

        var sessionsRepositoryMock = new Mock<IGameSessionsRepository>(MockBehavior.Strict);
        sessionsRepositoryMock
            .Setup(x => x.AddAsync(sessionMock.Object, TestContext.Current.CancellationToken))
            .Returns(ValueTask.CompletedTask);

        var gameFactoryMock = new Mock<IGameSessionFactory>(MockBehavior.Strict);
        gameFactoryMock.Setup(x => x.CreateAsync(setup, TestContext.Current.CancellationToken))
            .ReturnsAsync(sessionMock.Object);

        var loaders = new List<IGameLoader>
        {
            new ClassicPlayersLoader(),
            new ZonesLoader(zonesRepoMock.Object, zonesFactoryMock.Object),
            new DecksLoader(decksRepoMock.Object, cardsFactoryMock.Object)
        };

        var startupService = new GameStartupService(
            sessionsRepositoryMock.Object,
            loaders,
            gameFactoryMock.Object);

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