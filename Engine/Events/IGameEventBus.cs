namespace Centuriin.CardGame.Core.Engine.Events;

/// <summary>
/// Combines reading and writing queued domain events.
/// </summary>
public interface IGameEventBus : IGameEventBusReader, IGameEventBusWriter
{
}