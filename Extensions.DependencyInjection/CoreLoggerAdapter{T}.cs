using Microsoft.Extensions.Logging;

using MyEngine = Centuriin.CardGame.Core.Engine.Observability.Logging;

namespace Centuriin.CardGame.Core.Extensions.DependencyInjection;

internal sealed class CoreLoggerAdapter<T> : MyEngine.ICoreLogger<T>
{
    private readonly ILogger<T> _logger;

    public CoreLoggerAdapter(ILogger<T> logger)
    {
        ArgumentNullException.ThrowIfNull(logger);
        _logger = logger;
    }

    /// <inheritdoc/>
    public bool IsEnabled(MyEngine.LogLevel logLevel) => 
        _logger.IsEnabled(Map(logLevel));

    /// <inheritdoc/>
    public void LogDebug(string message, params object[] args) =>
        _logger.LogDebug(message, args);

    /// <inheritdoc/>
    public void LogError(string message, params object[] args) =>
        _logger.LogError(message, args);

    /// <inheritdoc/>
    public void LogInformation(string message, params object[] args) =>
        _logger.LogInformation(message, args);

    /// <inheritdoc/>
    public void LogTrace(string message, params object[] args) =>
        _logger.LogTrace(message, args);

    /// <inheritdoc/>
    public void LogWarning(string message, params object[] args) =>
        _logger.LogWarning(message, args);

    private static LogLevel Map(MyEngine.LogLevel logLevel) => logLevel switch
    {
        MyEngine.LogLevel.Trace => LogLevel.Trace,
        MyEngine.LogLevel.Debug => LogLevel.Debug,
        MyEngine.LogLevel.Information => LogLevel.Information,
        MyEngine.LogLevel.Warning => LogLevel.Warning,
        MyEngine.LogLevel.Error => LogLevel.Error,

        _ => throw new InvalidOperationException(
            $"Log level {logLevel} can't be mapped to core log level.")
    };
}