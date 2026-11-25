using Centuriin.CardGame.Core.Engine.Events.Domain;

namespace Centuriin.CardGame.Core.Engine.Loading.Configuration.Pipeline;

/// <summary>
/// Selects the events handled by a configured game system.
/// </summary>
public interface IConfigurableSystem
{
    /// <summary>
    /// Gets the parent pipeline configuration to continue configuring systems.
    /// </summary>
    public IConfigurableGamePipeline Pipeline { get; }

    /// <summary>
    /// Registers this system's consuming and producing subscriptions for the selected event type.
    /// </summary>
    /// <typeparam name="TEvent">
    /// The event type handled by this contract.
    /// </typeparam>
    /// <returns>
    /// This system configuration for further subscriptions.
    /// </returns>
    public IConfigurableSystem ForEvent<TEvent>()
        where TEvent : IGameEvent;
}