using System;
using System.Collections.Generic;
using System.Text;

using Centuriin.CardGame.Core.Common.Cards;
using Centuriin.CardGame.Core.Common.Engine;

using FluentAssertions;

using Xunit;

namespace Centuriin.CardGame.Core.Common;

public sealed class GameStateTests
{
    [Fact]
    public void Test()
    {
        // Arrange
        var spaceId = new SpaceId(1);
        var cards = CardSets.Default36Set;

        var state = new GameState(
            new Dictionary<SpaceId, List<ICard>>()
            {
                { spaceId, [.. cards] }
            },
            new RandomCardPicker());

        // Act
        state.AddCardToSpace(spaceId, cards.First());

        // Assert
        state._cardSpaces.SequenceEqual(
            new Dictionary<SpaceId, List<ICard>> 
            {
                { spaceId, [cards.First(), .. cards] }
            }).Should().BeTrue();
    }
}