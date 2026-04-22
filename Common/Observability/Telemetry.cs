using System.Diagnostics;

using Centuriin.CardGame.Core.Common.Events;

namespace Centuriin.CardGame.Core.Common.Observability;

public static class Telemetry
{
    public const string ACTIVITY_SOURCE_NAME = "Centuriin.CardGame.Core";

    private static ActivitySource Source { get; } =
        new ActivitySource(ACTIVITY_SOURCE_NAME);

    internal static IDisposable? StartActivity(IGameEvent @event)
    {
        ArgumentNullException.ThrowIfNull(@event);

        var activity = Source.StartActivity("ApplyEvent");
        activity?.SetTag("event.type", @event.GetType().Name);
        activity?.SetTag("game.id", @event.GameId.Value);

        return activity;
    }
}
