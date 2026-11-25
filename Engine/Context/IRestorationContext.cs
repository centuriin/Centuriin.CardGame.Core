using Centuriin.CardGame.Core.Engine.Events.Domain;

namespace Centuriin.CardGame.Core.Engine.Context;

/// <summary>
/// Provides a batch of recorded events to apply during restoration.
/// </summary>
public interface IRestorationContext : IGameContext
{
    /// <summary>
    /// Gets the recorded event batch to apply during restoration.
    /// </summary>
    public IGameEventsBatch RestorationBatch { get; }
}