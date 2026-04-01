using Centuriin.CardGame.Core.Common;
using Centuriin.CardGame.Core.Common.Components;
using Centuriin.CardGame.Core.Common.Entities.Cards;
using Centuriin.CardGame.Core.Common.Entities.Players;
using Centuriin.CardGame.Core.Common.Events;
using Centuriin.Centuriin.Core.Common;

using FluentAssertions;

using Moq;

using Xunit;

namespace Centuriin.CardGame.Core.Tests;

public sealed class ClassicDealerServiceTests
{
    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    public async Task CanNotDealCardsWithInvalidCountAsync(int count)
    {
        // Arrange
        var service = new ClassicDealerService(
            Mock.Of<IGame>(MockBehavior.Strict));

        // Act
        var exception = await Record.ExceptionAsync(async () => 
            await service.DealCardsAsync(count, TestContext.Current.CancellationToken));

        // Assert
        exception.Should().BeOfType<ArgumentOutOfRangeException>();
    }

    [Fact]
    public async Task DealCardsAsyncShouldPublishCorrectAmountOfEventsAndFinalizeAsync()
    {
        // Arrange
        var gameId = new GameId(Guid.NewGuid());
        var player1Id = new PlayerId(Guid.NewGuid());
        var player2Id = new PlayerId(Guid.NewGuid());

        var gameStateMock = new Mock<IGameState>(MockBehavior.Strict);
        gameStateMock
            .Setup(x => x.GameId)
            .Returns(gameId);
        gameStateMock
            .Setup(x => x.Query<Player>())
            .Returns([new(PlayerId.System), new(player1Id), new(player2Id)]);
        gameStateMock
            .Setup(x => x.Query<Card>())
            .Returns(
            [.. 
                Enumerable.Range(1, 10)
                .Select(i =>
                {
                    var card = new Card(new CardId(i));
                    card.Add(new OwnerComponent(PlayerId.System));
                    return card;
                })
            ]);

        var cardDealtCalls = 0;
        var gameStartedCalls = 0;
        var dealtPlayerIds = new List<PlayerId>();

        var game = new Mock<IGame>(MockBehavior.Strict);
        game
            .SetupGet(x => x.State)
            .Returns(gameStateMock.Object);
        game
            .Setup(x => x.ApplyAsync(
                It.IsAny<CardDealtEvent>(), 
                TestContext.Current.CancellationToken))
            .Returns(Task.CompletedTask)
            .Callback<IGameEvent, CancellationToken>((@event, _) =>
            {
                cardDealtCalls++;
                dealtPlayerIds.Add(((CardDealtEvent)@event).NewOwnerId);
            });
        game
            .Setup(x => x.ApplyAsync(
                It.IsAny<GameStartedEvent>(), 
                TestContext.Current.CancellationToken))
            .Returns(Task.CompletedTask)
            .Callback(() => gameStartedCalls++);

        var service = new ClassicDealerService(game.Object);

        // Act
        await service.DealCardsAsync(count: 2, TestContext.Current.CancellationToken);

        // Assert
        cardDealtCalls.Should().Be(2);
        gameStartedCalls.Should().Be(1);

        dealtPlayerIds.Should().Contain(player1Id);
        dealtPlayerIds.Should().Contain(player2Id);
        dealtPlayerIds.Should().NotContain(PlayerId.System);
    }
}
