using Centuriin.CardGame.Core.Common.Cards;

using Xunit;

namespace Common.Tests;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        // Arrange
        var suit = (CardSuit)default;

        var isDefined = Enum.IsDefined(suit);

        Assert.Equal((int)suit, 0);
    }
}
