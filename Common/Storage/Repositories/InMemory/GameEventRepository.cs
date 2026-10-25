using Centuriin.CardGame.Core.Common.Events.Application;
using Centuriin.CardGame.Core.Common.Events.Dispatching;
using Centuriin.CardGame.Core.Common.Events.Domain;
using Centuriin.CardGame.Core.Common.Storage.Repositories;

namespace Centuriin.CardGame.Core.Common.Storage.Repositories.InMemory;

public sealed class GameEventRepository : IGameEventsRepository, IDisposable
{
    private bool _disposed;

    private readonly IAsyncDispatcher<IApplicationEvent> _dispatcher;

    public readonly List<IGameEventsBatch> Events = [];

    public GameEventRepository(IAsyncDispatcher<IApplicationEvent> dispatcher)
    {
        ArgumentNullException.ThrowIfNull(dispatcher);
        _dispatcher = dispatcher;

        _dispatcher.Register(this);
    }

    public Task AddAsync(IGameEventsBatch eventsBatch, CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(eventsBatch);

        ObjectDisposedException.ThrowIf(_disposed, this);

        Events.Add(eventsBatch);

        return Task.CompletedTask;
    }

    public async Task OnEventAsync(
        NewGameEventsBatchCreatedEvent @event, 
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(@event);

        token.ThrowIfCancellationRequested();

        ObjectDisposedException.ThrowIf(_disposed, this);

        await AddAsync(@event.Batch, token);
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        _dispatcher.Unregister(this);

        _disposed = true;
    }
}
