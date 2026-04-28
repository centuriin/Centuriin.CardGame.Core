using System.Diagnostics;

using Centuriin.CardGame.Core.Common.Commands;
using Centuriin.CardGame.Core.Common.Events;
using Centuriin.CardGame.Core.Common.World;

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

    internal static IDisposable? StartActivity(ICommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        var activity = Source.StartActivity("ExecuteCommand");
        activity?.SetTag("command.type", command.GetType().Name);
        activity?.SetTag("game.id", command.GameId.Value);
        activity?.SetTag("actor.id", command.Actor.Value);

        return activity;
    }

    internal static IDisposable? StartGameActivity(IGame game)
    {
        ArgumentNullException.ThrowIfNull(game);

        var activity = Source.StartActivity("GameStart");
        activity?.SetTag("game.id", game.Id.Value);

        return activity;
    }
}
