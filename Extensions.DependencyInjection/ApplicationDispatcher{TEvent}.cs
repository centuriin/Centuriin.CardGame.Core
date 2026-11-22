using Centuriin.CardGame.Core.Application;

using Microsoft.Extensions.DependencyInjection;

namespace Centuriin.CardGame.Core.Extensions.DependencyInjection;

internal sealed class ApplicationDispatcher<TEvent> : IApplicationDispatcher<TEvent>
{
    private readonly IServiceScopeFactory _scopeFactory;

    public ApplicationDispatcher(IServiceScopeFactory scopeFactory)
    {
        ArgumentNullException.ThrowIfNull(scopeFactory);
        _scopeFactory = scopeFactory;
    }

    /// <inheritdoc/>
    public async Task PublishAsync<T>(T @event, CancellationToken token)
        where T : TEvent
    {
        ArgumentNullException.ThrowIfNull(@event);

        token.ThrowIfCancellationRequested();

        if (@event.GetType() != typeof(T))
        {
            throw new ArgumentException(
                "Need concrete type for event.",
                nameof(@event));
        }

        await using var scope = _scopeFactory.CreateAsyncScope();

        foreach (var handler in scope.ServiceProvider
            .GetServices<IApplicationHandler<T>>())
        {
            await handler.HandleAsync(@event, token);
        }
    }
}
