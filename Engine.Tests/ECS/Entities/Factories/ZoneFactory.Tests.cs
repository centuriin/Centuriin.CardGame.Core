using Centuriin.CardGame.Core.Engine.ECS.Components;
using Centuriin.CardGame.Core.Engine.ECS.Entities;
using Centuriin.CardGame.Core.Engine.ECS.Entities.Factories;
using Centuriin.CardGame.Core.Engine.ECS.Templates;

using FluentAssertions;

using Moq;

using Xunit;

namespace Centuriin.CardGame.Core.Engine.Tests.ECS.Entities.Factories;

public sealed class ZoneFactoryTests
{
    [Fact(DisplayName = "Can create distinct zone entities in template order with allocated identifiers.")]
    [Trait("Category", "Unit")]
    public void CanCreateEntitiesFromTemplates()
    {
        // Arrange
        var owner = new OwnerComponent(new(7));

        ZoneTemplate template = new(
            new(11),
            [owner],
            ZoneScope.PerPlayer);

        EntityId[] ids = [new(43), new(44)];

        var allocationCalls = 0;
        var provider = new Mock<IEntityIdProvider>(MockBehavior.Strict);
        provider.Setup(x => x.GetNextId())
            .Returns(() => ids[allocationCalls++]);

        var factory = new ZoneFactory(provider.Object);

        // Act
        var entities = factory.Create(template, template);

        // Assert
        allocationCalls.Should().Be(2);
        entities.Select(x => x.Id).Should().Equal(new EntityId(43), new EntityId(44));
        entities[0].Entity.Should().NotBeSameAs(entities[1].Entity);
        entities[0].Get<OwnerComponent>().Should().BeSameAs(owner);
        entities[1].Get<OwnerComponent>().Should().BeSameAs(owner);
        entities[0].Get<TemplateComponent>().TemplateId.Should().Be(template.Id);
        entities[1].Get<TemplateComponent>().TemplateId.Should().Be(template.Id);
    }

    [Fact(DisplayName = "Can edit one created entity without modifying another entity or its template.")]
    [Trait("Category", "Unit")]
    public void CanEditCreatedEntitiesIndependently()
    {
        // Arrange
        var owner = new OwnerComponent(new(7));

        ZoneTemplate template = new(
            new(11),
            [owner],
            ZoneScope.PerPlayer);

        EntityId[] ids = [new(1), new(2)];

        var allocationCalls = 0;
        var provider = new Mock<IEntityIdProvider>(MockBehavior.Strict);
        provider.Setup(x => x.GetNextId())
            .Returns(() => ids[allocationCalls++]);

        var factory = new ZoneFactory(provider.Object);

        var entities = factory.Create(template, template);

        // Act
        entities[0].Set(new OwnerComponent(new(8)));

        // Assert
        allocationCalls.Should().Be(2);
        entities[0].Get<OwnerComponent>().CurrentOwnerId.Should().Be(new EntityId(8));
        entities[1].Get<OwnerComponent>().Should().BeSameAs(owner);
        template.Components.Should().Equal(owner);
    }

    [Fact(DisplayName = "Can create an empty batch without allocating entity identifiers.")]
    [Trait("Category", "Unit")]
    public void CanCreateEmptyBatch()
    {
        // Arrange
        var factory = new ZoneFactory(Mock.Of<IEntityIdProvider>(MockBehavior.Strict));

        // Act
        var entities = factory.Create([]);

        // Assert
        entities.Should().BeEmpty();
    }
}
