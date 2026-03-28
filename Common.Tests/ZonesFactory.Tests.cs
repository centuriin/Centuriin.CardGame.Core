using Centuriin.CardGame.Core.Common.Components;
using Centuriin.CardGame.Core.Common.Entities.Zones;
using Centuriin.CardGame.Core.Common.Repositories;

using FluentAssertions;

using Moq;

using Xunit;

namespace Centuriin.CardGame.Core.Common.Tests;

public sealed class ZonesFactoryTests
{
    [Fact]
    public async Task CreateAsyncShouldAddCreatedZonesToGameStateAsync()
    {
        // Arrange
        var gameTypeId = new GameTypeId(1);
        var templates = new List<ZoneTemplate>
        {
            new(new(101), [new FakeComponent()]),
            new(new(102), [new FakeComponent()])
        };

        var repository = new Mock<IZoneTemplatesRepository>(MockBehavior.Strict);
        repository
            .Setup(x => x.GetTemplatesByGameTypeAsync(gameTypeId, TestContext.Current.CancellationToken))
            .ReturnsAsync(templates);

        var factory = new ZonesFactory(repository.Object);

        // Act
        var zones = await factory.CreateAsync(gameTypeId, TestContext.Current.CancellationToken);

        // Assert
        zones.Should().HaveCount(2);

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
                second.Get<TemplateComponent>().TemplateId.Value.Should().Be(102);
            });
    }

    public sealed record class FakeComponent() : ComponentBase;
}