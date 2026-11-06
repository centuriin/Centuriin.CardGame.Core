using System.Diagnostics;

using Centuriin.CardGame.Core.Common.Commands;
using Centuriin.CardGame.Core.Common.Root;

namespace Centuriin.CardGame.Core.Common.Observability;

public static class Telemetry
{
    public const string ACTIVITY_SOURCE_NAME = "Centuriin.CardGame.Core";

    private static ActivitySource Source { get; } =
        new ActivitySource(ACTIVITY_SOURCE_NAME);

    internal static Activity? StartGameSessionActivity(IGameSession session)
    {
        ArgumentNullException.ThrowIfNull(session);

        var activity = Source.StartActivity(
            "GameSession",
            ActivityKind.Internal);

        activity?.SetTag("game.id", session.GameId.Value);

        return activity;
    }

    internal static IDisposable? StartCommandActivity(ICommand command, ActivityContext context)
    {
        ArgumentNullException.ThrowIfNull(command);

        var activity = Source.StartActivity(
            "ExecuteCommand",
            ActivityKind.Internal,
            parentContext: context);

        activity?.SetTag("command.type", command.GetType().Name);
        activity?.SetTag("game.id", command.GameId.Value);
        activity?.SetTag("actor.id", command.Actor.Value);

        return activity;
    }

    internal static IDisposable? StartGameActivity(
        GameId gameId, 
        ActivityContext context)
    {
        var activity = Source.StartActivity(
            "StartGame",
            ActivityKind.Internal,
            parentContext: context);

        activity?.SetTag("game.id", gameId.Value);

        return activity;
    }
}
