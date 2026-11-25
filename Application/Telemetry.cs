using System.Diagnostics;

using Centuriin.CardGame.Core.Engine;
using Centuriin.CardGame.Core.Engine.Commands;

namespace Centuriin.CardGame.Core.Application;

/// <summary>
/// Provides activity instrumentation for sessions, game startup, and commands.
/// </summary>
public static class Telemetry
{
    /// <summary>
    /// The activity source name used to subscribe to engine instrumentation.
    /// </summary>
    public const string ACTIVITY_SOURCE_NAME = "Centuriin.CardGame.Core.Engine";

    private static ActivitySource Source { get; } =
        new ActivitySource(ACTIVITY_SOURCE_NAME);

    /// <summary>
    /// Starts an activity that measures the lifetime of a game session.
    /// </summary>
    /// <param name="session">
    /// The game session associated with the operation.
    /// </param>
    /// <returns>
    /// The started activity, or <see langword="null"/> when no listener records it.
    /// </returns>
    internal static Activity? StartGameSessionActivity(IGameSession session)
    {
        ArgumentNullException.ThrowIfNull(session);

        var activity = Source.StartActivity(
            "GameSession",
            ActivityKind.Internal);

        activity?.SetTag("game.id", session.GameId.Value);

        return activity;
    }

    /// <summary>
    /// Starts an activity for command execution using the session's trace context.
    /// </summary>
    /// <param name="command">
    /// The command to validate or execute.
    /// </param>
    /// <param name="context">
    /// The parent activity context used for tracing.
    /// </param>
    /// <returns>
    /// The started activity, or <see langword="null"/> when no listener records it.
    /// </returns>
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

    /// <summary>
    /// Starts an activity for game startup using the session's trace context.
    /// </summary>
    /// <param name="gameId">
    /// The identifier of the game.
    /// </param>
    /// <param name="context">
    /// The parent activity context used for tracing.
    /// </param>
    /// <returns>
    /// The started activity, or <see langword="null"/> when no listener records it.
    /// </returns>
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
