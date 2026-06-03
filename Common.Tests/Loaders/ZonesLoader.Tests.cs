using Centuriin.CardGame.Core.Common.Components;
using Centuriin.CardGame.Core.Common.Components.Players;
using Centuriin.CardGame.Core.Common.Components.Zones;
using Centuriin.CardGame.Core.Common.Entities.Factories;
using Centuriin.CardGame.Core.Common.Entities.Players;
using Centuriin.CardGame.Core.Common.Entities.Zones;
using Centuriin.CardGame.Core.Common.Loaders;
using Centuriin.CardGame.Core.Common.Repositories;
using Centuriin.CardGame.Core.Common.Templates;
using Centuriin.CardGame.Core.Common.World;

using FluentAssertions;

using Moq;

using Xunit;

namespace Centuriin.CardGame.Core.Common.Tests.Loaders;

public sealed class ZonesLoaderTests
{
    [Fact]
    public async Task LoadAsyncShouldAssignOwnersToHandAndDeckZonesAndAddToGameStateAsync()
    {
        // Arrange
        var gameTypeId = new GameTypeId(1);

        var participant = new Player(new(1));
        participant.Add(new PlayerRoleComponent(PlayerRole.Participant));

        var deckOwner = new Player(new(2));
        deckOwner.Add(new PlayerRoleComponent(PlayerRole.Participant | PlayerRole.Bank));

        var handZone1 = new Zone(new(10));
        handZone1.Add(new ZoneRoleComponent(ZoneRole.Hand));

        var handZone2 = new Zone(new(20));
        handZone2.Add(new ZoneRoleComponent(ZoneRole.Hand));

        var deckZone = new Zone(new(30));
        deckZone.Add(new ZoneRoleComponent(ZoneRole.Deck));

        var addedEntities = new List<Zone>();
        var gameStateMock = new Mock<IGameState>(MockBehavior.Strict);
        gameStateMock
            .Setup(x => x.Query<Player>())
            .Returns([participant, deckOwner]);
        gameStateMock
            .Setup(x => x.AddEntity(It.IsAny<Zone>()))
            .Callback<Zone>(addedEntities.Add);

        var zoneTemplateIds = new TemplateId[] { new(11), new(22) };
        var zonesRepo = new Mock<IZonesRepository>(MockBehavior.Strict);
        zonesRepo
            .Setup(x => x.GetZoneTemplateIdsAsync(gameTypeId, TestContext.Current.CancellationToken))
            .ReturnsAsync(zoneTemplateIds);

        var zonesFactoryMock = new Mock<IZoneFactory>(MockBehavior.Strict);
        zonesFactoryMock
            .Setup(x => x.CreateAsync(
                It.Is<IReadOnlyCollection<TemplateId>>(x =>
                    x.Count == 2
                    && x.All(x => zoneTemplateIds.Contains(x))),
                    activePlayersCount: 2,
                TestContext.Current.CancellationToken))
            .ReturnsAsync([handZone1, handZone2, deckZone]);

        var loader = new ZonesLoader(
            zonesRepo.Object,
            zonesFactoryMock.Object);

        // Act
        await loader.LoadAsync(
            new(gameTypeId, [new PlayerId(Guid.NewGuid()), new PlayerId(Guid.NewGuid())]),
            gameStateMock.Object,
            TestContext.Current.CancellationToken);

        // Assert
        addedEntities.Should().HaveCount(3);

        addedEntities.Should().SatisfyRespectively(
            first => first
                .Get<OwnerComponent>().CurrentOwnerId
                .Should().BeEquivalentTo(participant.Id),
            second => second
                .Get<OwnerComponent>().CurrentOwnerId
                .Should().BeEquivalentTo(deckOwner.Id),
            third => third
                .Get<OwnerComponent>().CurrentOwnerId
                .Should().BeEquivalentTo(deckOwner.Id));
    }

    [Fact]
    public async Task LoadAsyncShouldHandleEmptyZonesFromFactoryAsync()
    {
        // Arrange
        var gameTypeId = new GameTypeId(2);

        var addedEntities = new List<Zone>();
        var gameStateMock = new Mock<IGameState>(MockBehavior.Strict);
        gameStateMock
            .Setup(x => x.Query<Player>())
            .Returns([]);
        gameStateMock
            .Setup(x => x.AddEntity(It.IsAny<Zone>()))
            .Callback<Zone>(addedEntities.Add);

        var zonesRepo = new Mock<IZonesRepository>(MockBehavior.Strict);
        zonesRepo
            .Setup(x => x.GetZoneTemplateIdsAsync(gameTypeId, TestContext.Current.CancellationToken))
            .ReturnsAsync([]);

        var zonesFactoryMock = new Mock<IZoneFactory>(MockBehavior.Strict);
        zonesFactoryMock
            .Setup(x => x.CreateAsync(
                It.Is<IReadOnlyCollection<TemplateId>>(x => x.Count == 0),
                activePlayersCount: 0,
                TestContext.Current.CancellationToken))
            .ReturnsAsync([]);

        var loader = new ZonesLoader(
            zonesRepo.Object,
            zonesFactoryMock.Object);

        // Act
        await loader.LoadAsync(
            new(gameTypeId, []),
            gameStateMock.Object,
            TestContext.Current.CancellationToken);

        // Assert
        addedEntities.Should().BeEmpty();
    }
}
