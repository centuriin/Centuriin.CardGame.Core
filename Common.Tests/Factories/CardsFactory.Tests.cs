using Moq;

using Xunit;

using FluentAssertions;

using Centuriin.CardGame.Core.Common.Components;
using Centuriin.CardGame.Core.Common.Entities.Cards;
using Centuriin.CardGame.Core.Common.Repositories;
using Centuriin.CardGame.Core.Common.Entities;

namespace Centuriin.CardGame.Core.Common.Factories;

public sealed class CardsFactoryTests
{
    [Fact]
    public async Task CreateAsyncShouldLoadTemplatesAndCreateCardsWithIncrementalIdsAsync()
    {
        // Arrange
        var templateId1 = new TemplateId(101);
        var templateId2 = new TemplateId(102);
        var templateIds = new List<TemplateId> { templateId1, templateId1, templateId2 };

        var repoMock = new Mock<ICardTemplatesRepository>(MockBehavior.Strict);
        repoMock
            .Setup(x => x.GetTemplatesByIdsAsync(
                It.Is<IReadOnlySet<TemplateId>>(x => x.Count == 2),
                TestContext.Current.CancellationToken))
            .ReturnsAsync(new Dictionary<TemplateId, CardTemplate>
            {
                { templateId1, new CardTemplate(templateId1, [new FakeComponent()]) },
                { templateId2, new CardTemplate(templateId2, [new FakeComponent()]) }
            });

        var factory = new CardsFactory(repoMock.Object);

        // Act
        var result = await factory.CreateAsync(templateIds, TestContext.Current.CancellationToken);

        // Assert
        result.Should().HaveCount(3);

        result.Should().SatisfyRespectively(
            first =>
            {
                first.Id.Value.Should().Be(1);
                first.Get<TemplateComponent>().TemplateId.Should().BeEquivalentTo(templateId1);
                first.Has<FakeComponent>().Should().BeTrue();
            },
            second =>
            {
                second.Id.Value.Should().Be(2);
                second.Get<TemplateComponent>().TemplateId.Should().BeEquivalentTo(templateId1);
                second.Has<FakeComponent>().Should().BeTrue();
            },
            third =>
            {
                third.Id.Value.Should().Be(3);
                third.Get<TemplateComponent>().TemplateId.Should().BeEquivalentTo(templateId2);
                third.Has<FakeComponent>().Should().BeTrue();
            });
    }

    [Fact]
    public async Task CreateAsyncShouldHandleEmptyTemplatesAsync()
    {
        // Arrange
        var repoMock = new Mock<ICardTemplatesRepository>(MockBehavior.Strict);
        repoMock
            .Setup(x => x.GetTemplatesByIdsAsync(
                It.IsAny<IReadOnlySet<TemplateId>>(), 
                TestContext.Current.CancellationToken))
            .ReturnsAsync(new Dictionary<TemplateId, CardTemplate>());

        var factory = new CardsFactory(repoMock.Object);

        // Act
        var result = await factory.CreateAsync([], TestContext.Current.CancellationToken);

        // Assert
        result.Should().BeEmpty();
    }

    public sealed record FakeComponent : ComponentBase;
}