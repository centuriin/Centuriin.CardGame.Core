using Centuriin.CardGame.Core.Engine.ECS.Entities;
using Centuriin.CardGame.Core.Engine.ECS.Entities.Cards;
using Centuriin.CardGame.Core.Engine.Loading;
using Centuriin.CardGame.Core.Engine.Root.State;

using FluentAssertions;

using Moq;

using Xunit;

namespace Centuriin.CardGame.Core.Engine.Tests.Loading;

public sealed class EntityLoaderTests
{
    [Fact(DisplayName = "Can load the requested entity type into the target game state.")]
    [Trait("Category", "Unit")]
    public void CanLoadSupportedEntityType()
    {
        // Arrange
        Card[] cards = [new(new(1)), new(new(2))];

        var source = new Mock<IReadOnlyEntitiesCollection>(MockBehavior.Strict);
        source.Setup(x => x.Query<Card>())
            .Returns(cards);

        var added = new List<EntityBase>();
        var addCalls = 0;
        var destination = new Mock<IEntitiesCollection>(MockBehavior.Strict);
        destination.Setup(x => x.Add(It.IsAny<IEnumerable<EntityBase>>()))
            .Callback<IEnumerable<EntityBase>>(entities =>
            {
                addCalls++;
                added.AddRange(entities);
            });

        var state = new Mock<IGameState>(MockBehavior.Strict);
        state.SetupGet(x => x.Entities)
            .Returns(destination.Object);

        var loader = new EntityLoader<Card>();

        // Act
        loader.Load(source.Object, state.Object);

        // Assert
        addCalls.Should().Be(2);
        added.Should().Equal(cards);
    }

    [Fact(DisplayName = "Can load an empty source without accessing the target entities.")]
    [Trait("Category", "Unit")]
    public void CanLoadEmptySource()
    {
        // Arrange
        var source = new Mock<IReadOnlyEntitiesCollection>(MockBehavior.Strict);
        source.Setup(x => x.Query<Card>())
            .Returns([]);

        var state = Mock.Of<IGameState>(MockBehavior.Strict);

        var loader = new EntityLoader<Card>();

        // Act
        var exception = Record.Exception(() =>
            loader.Load(source.Object, state));

        // Assert
        exception.Should().BeNull();
    }

    [Fact(DisplayName = "Cannot load entities without a source collection.")]
    [Trait("Category", "Unit")]
    public void CanNotLoadWithoutSource()
    {
        // Arrange
        var loader = new EntityLoader<Card>();

        // Act
        var exception = Record.Exception(() =>
            loader.Load(null!, Mock.Of<IGameState>(MockBehavior.Strict)));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = "Cannot load entities without target game state.")]
    [Trait("Category", "Unit")]
    public void CanNotLoadWithoutGameState()
    {
        // Arrange
        var loader = new EntityLoader<Card>();

        // Act
        var exception = Record.Exception(() =>
            loader.Load(Mock.Of<IReadOnlyEntitiesCollection>(MockBehavior.Strict), null!));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }
}
