using Moq;

using Xunit;

using FluentAssertions;

using Centuriin.CardGame.Core.Common.Components;
using Centuriin.CardGame.Core.Common.Entities.Players;
using Centuriin.CardGame.Core.Common.Entities.Zones;
using Centuriin.CardGame.Core.Common.Factories;
using Centuriin.CardGame.Core.Common.Components.Players;
using Centuriin.CardGame.Core.Common.Components.Zones;

namespace Centuriin.CardGame.Core.Common.Loaders;

public sealed class ZonesLoaderTests
{
    [Fact]
    public async Task LoadAsyncShouldAssignOwnersToHandAndDeckZonesAndAddToGameStateAsync()
    {
        // Arrange
        var gameTypeId = new GameTypeId(1);

        var participant = new Player(new PlayerId(Guid.NewGuid()));
        participant.Add(new PlayerRoleComponent(PlayerRole.Participant));

        var deckOwner = new Player(new PlayerId(Guid.NewGuid()));
        deckOwner.Add(new PlayerRoleComponent(PlayerRole.Participant | PlayerRole.Bank));

        var handZone1 = new Zone(new ZoneId(10));
        handZone1.Add(new ZoneRoleComponent(ZoneRole.Hand));

        var handZone2 = new Zone(new ZoneId(20));
        handZone2.Add(new ZoneRoleComponent(ZoneRole.Hand));

        var deckZone = new Zone(new ZoneId(30));
        deckZone.Add(new ZoneRoleComponent(ZoneRole.Deck));

        var addedEntities = new List<Zone>();
        var gameStateMock = new Mock<IGameState>(MockBehavior.Strict);
        gameStateMock
            .Setup(x => x.Query<Player>())
            .Returns([participant, deckOwner]);
        gameStateMock
            .Setup(x => x.AddEntity<Zone, ZoneId>(It.IsAny<Zone>()))
            .Callback<Zone>(addedEntities.Add);

        var zonesFactoryMock = new Mock<IZonesFactory>(MockBehavior.Strict);
        zonesFactoryMock
            .Setup(x => x.CreateAsync(gameTypeId, TestContext.Current.CancellationToken))
            .ReturnsAsync([handZone1, handZone2, deckZone]);

        var loader = new ZonesLoader(gameStateMock.Object, zonesFactoryMock.Object);

        // Act
        await loader.LoadAsync(gameTypeId, TestContext.Current.CancellationToken);

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
            .Setup(x => x.AddEntity<Zone, ZoneId>(It.IsAny<Zone>()))
            .Callback<Zone>(addedEntities.Add);

        var zonesFactoryMock = new Mock<IZonesFactory>(MockBehavior.Strict);
        zonesFactoryMock
            .Setup(x => x.CreateAsync(gameTypeId, TestContext.Current.CancellationToken))
            .ReturnsAsync([]);

        var loader = new ZonesLoader(gameStateMock.Object, zonesFactoryMock.Object);

        // Act
        await loader.LoadAsync(gameTypeId, TestContext.Current.CancellationToken);

        // Assert
        addedEntities.Should().BeEmpty();
    }
}
