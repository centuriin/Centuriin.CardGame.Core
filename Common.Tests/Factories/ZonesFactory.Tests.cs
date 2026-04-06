using Centuriin.CardGame.Core.Common.Components;
using Centuriin.CardGame.Core.Common.Entities.Zones;
using Centuriin.CardGame.Core.Common.Repositories;

using FluentAssertions;

using Moq;

using Xunit;

namespace Centuriin.CardGame.Core.Common.Factories;

public sealed class ZonesFactoryTests
{
    [Fact]
    public async Task CreateAsyncShouldAddCreatedZonesToGameStateAsync()
    {
        // Arrange
        var templateId1 = new TemplateId(101);
        var templateId2 = new TemplateId(102);
        var templateIds = new List<TemplateId> { templateId1, templateId1, templateId2 };

        var repository = new Mock<ITemplatesRepository<ZoneTemplate>>(MockBehavior.Strict);
        repository
            .Setup(x => x.GetTemplatesByIdsAsync(
                It.Is<IReadOnlyCollection<TemplateId>>(x => x.Count == 3),
                TestContext.Current.CancellationToken))
            .ReturnsAsync(
                [
                    new ZoneTemplate(templateId1, [new FakeComponent()]),
                    new ZoneTemplate(templateId2, [new FakeComponent()])
                ]);

        var factory = new ZonesFactory(repository.Object);

        // Act
        var zones = await factory.CreateAsync(templateIds, TestContext.Current.CancellationToken);

        // Assert
        zones.Should().HaveCount(3);

        zones.Should().SatisfyRespectively(
            first =>
            {
                first.Id.Value.Should().Be(1);
                first.Has<FakeComponent>().Should().BeTrue();
                first.Get<TemplateComponent>().TemplateId.Value.Should().Be(101);
            },
            second =>
            {
                second.Id.Value.Should().Be(2);
                second.Has<FakeComponent>().Should().BeTrue();
                second.Get<TemplateComponent>().TemplateId.Value.Should().Be(101);
            },
            last =>
            {
                last.Id.Value.Should().Be(3);
                last.Has<FakeComponent>().Should().BeTrue();
                last.Get<TemplateComponent>().TemplateId.Value.Should().Be(102);
            });
    }

    public sealed record class FakeComponent() : ComponentBase;
}