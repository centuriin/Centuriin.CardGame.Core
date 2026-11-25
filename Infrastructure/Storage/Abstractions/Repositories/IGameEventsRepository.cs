using Centuriin.CardGame.Core.Engine.Events.Domain;

namespace Centuriin.CardGame.Core.Infrastructure.Storage.Abstractions.Repositories;

/// <summary>
/// Stores batches of domain events.
/// </summary>
public interface IGameEventsRepository
{
    /// <summary>
    /// Stores an ordered batch of domain events.
    /// </summary>
    /// <param name="eventsBatch">
    /// The ordered batch of domain events to store.
    /// </param>
    /// <param name="token">
    /// The cancellation token for the operation.
    /// </param>
    /// <returns>
    /// A task that completes when the operation finishes.
    /// </returns>
    public Task AddAsync(
        IGameEventsBatch eventsBatch,
        CancellationToken token);
}