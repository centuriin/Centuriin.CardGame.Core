namespace Centuriin.CardGame.Core.Engine.Observability.Logging;

/// <summary>
/// Writes engine diagnostic messages without depending on a logging framework.
/// </summary>
public interface ICoreLogger
{
    /// <summary>
    /// Checks whether messages at the supplied severity are enabled.
    /// </summary>
    /// <param name="logLevel">
    /// The severity of the diagnostic message.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when the severity is enabled; otherwise, <see langword="false"/>.
    /// </returns>
    public bool IsEnabled(LogLevel logLevel);

    /// <summary>
    /// Writes an error message.
    /// </summary>
    /// <param name="message">
    /// The diagnostic message template.
    /// </param>
    /// <param name="args">
    /// The values used to format the diagnostic message.
    /// </param>
    public void LogError(string message, params object[] args);

    /// <summary>
    /// Writes a warning message.
    /// </summary>
    /// <param name="message">
    /// The diagnostic message template.
    /// </param>
    /// <param name="args">
    /// The values used to format the diagnostic message.
    /// </param>
    public void LogWarning(string message, params object[] args);

    /// <summary>
    /// Writes an informational message.
    /// </summary>
    /// <param name="message">
    /// The diagnostic message template.
    /// </param>
    /// <param name="args">
    /// The values used to format the diagnostic message.
    /// </param>
    public void LogInformation(string message, params object[] args);

    /// <summary>
    /// Writes a debug message.
    /// </summary>
    /// <param name="message">
    /// The diagnostic message template.
    /// </param>
    /// <param name="args">
    /// The values used to format the diagnostic message.
    /// </param>
    public void LogDebug(string message, params object[] args);

    /// <summary>
    /// Writes a trace message.
    /// </summary>
    /// <param name="message">
    /// The diagnostic message template.
    /// </param>
    /// <param name="args">
    /// The values used to format the diagnostic message.
    /// </param>
    public void LogTrace(string message, params object[] args);
}