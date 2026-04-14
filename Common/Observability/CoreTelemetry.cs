using System.Diagnostics;

using Centuriin.CardGame.Core.Common.Events;

namespace Centuriin.CardGame.Core.Common.Observability;

public static class CoreTelemetry
{
    private static ActivitySource Source { get; } =
        new ActivitySource(nameof(Centuriin.CardGame.Core));

    public static Activity? StartActivity(IGameEvent @event)
    {
        ArgumentNullException.ThrowIfNull(@event);

        var activity = Source.StartActivity("ApplyEvent");
        activity?.SetTag(@event.GetType().Name, @event);

        return activity;
    }
}
