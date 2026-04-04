using System.Diagnostics;

namespace Centuriin.CardGame.Core.Common.Logging;

public sealed class DebugLogger : IGameEngineLogger
{
    public static DebugLogger Instance { get; } = new();

    private DebugLogger() { }

    public bool IsEnabled(LogLevel logLevel) => true;

    public void LogError(string message, params object[] args) =>
        Debug.WriteLine($"[ERROR]: {message} {GetArgsLine(args)}");


    public void LogWarning(string message, params object[] args) =>
        Debug.WriteLine($"[WARNING]: {message} {GetArgsLine(args)}");

    public void LogInformation(string message, params object[] args) =>
        Debug.WriteLine($"[INFORMATION]: {message} {GetArgsLine(args)}");

    public void LogDebug(string message, params object[] args) =>
        Debug.WriteLine($"[DEBUG]: {message} {GetArgsLine(args)}");

    public void LogTrace(string message, params object[] args) =>
        Debug.WriteLine($"[TRACE]: {message} {GetArgsLine(args)}");

    private static string GetArgsLine(params object[] args) =>
        args.Length != 0
            ? $"| [{string.Join(", ", args)}]"
            : string.Empty;
}
