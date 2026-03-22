using System;
using System.Collections.Generic;
using System.Text;

using Centuriin.CardGame.Core.Common.Events;
using Centuriin.CardGame.Core.Common.Events.System;

using Moq;

using Xunit;

namespace Centuriin.CardGame.Core.Common;

public sealed class EventDispatcherTests
{
    [Fact]
    public async Task CanPublishAsync()
    {
        // Arrange
        var dispatcher = new EventDispatcher();

        var someEvent = Mock.Of<ISystemEvent>();

        dispatcher.Register<GameCreated>((@event, token) =>
            Task.FromResult<IReadOnlyCollection<ISystemEvent>>(
                [new CardInstanceCreated(new(Guid.NewGuid()), new(1), new(1))]));

        dispatcher.Register<CardInstanceCreated>((@event, token) =>
            Task.FromResult<IReadOnlyCollection<ISystemEvent>>([]));
        // Act
        await dispatcher.PublishAsync(
            new GameCreated(new(Guid.NewGuid()), new(1)),
            TestContext.Current.CancellationToken);

        // Assert

    }
}
