using System.Diagnostics;

namespace Centuriin.CardGame.Core.Engine.Observability.Logging;

/// <summary>
/// Writes engine diagnostic messages to the debugger output.
/// </summary>
/// <typeparam name="T">
/// The type used as the logging category.
/// </typeparam>
public sealed class DebugLogger<T> : ICoreLogger<T>
{
    /// <summary>
    /// Gets the shared instance.
    /// </summary>
    public static DebugLogger<T> Instance { get; } = new();

    private DebugLogger() { }

    /// <inheritdoc/>
    public bool IsEnabled(LogLevel logLevel) => true;

    /// <inheritdoc/>
    public void LogError(string message, params object[] args) =>
        Debug.WriteLine($"[ERROR]: {message} {GetArgsLine(args)}");


    /// <inheritdoc/>
    public void LogWarning(string message, params object[] args) =>
        Debug.WriteLine($"[WARNING]: {message} {GetArgsLine(args)}");

    /// <inheritdoc/>
    public void LogInformation(string message, params object[] args) =>
        Debug.WriteLine($"[INFORMATION]: {message} {GetArgsLine(args)}");

    /// <inheritdoc/>
    public void LogDebug(string message, params object[] args) =>
        Debug.WriteLine($"[DEBUG]: {message} {GetArgsLine(args)}");

    /// <inheritdoc/>
    public void LogTrace(string message, params object[] args) =>
        Debug.WriteLine($"[TRACE]: {message} {GetArgsLine(args)}");

    private static string GetArgsLine(params object[] args) =>
        args.Length != 0
            ? $"| [{string.Join(", ", args)}]"
            : string.Empty;
}
