using Microsoft.Extensions.Logging;

using Core = Centuriin.CardGame.Core.Common.Observability.Logging;

namespace Extensions.DependencyInjection;

public sealed class CoreLoggerAdapter<T> : Core.ICoreLogger<T>
{
    private readonly ILogger<T> _logger;

    public CoreLoggerAdapter(ILogger<T> logger)
    {
        ArgumentNullException.ThrowIfNull(logger);
        _logger = logger;
    }

    public bool IsEnabled(Core.LogLevel logLevel) => _logger.IsEnabled(Map(logLevel));

    public void LogDebug(string message, params object[] args) =>
        _logger.LogDebug(message, args);

    public void LogError(string message, params object[] args) =>
        _logger.LogError(message, args);

    public void LogInformation(string message, params object[] args) =>
        _logger.LogInformation(message, args);

    public void LogTrace(string message, params object[] args) =>
        _logger.LogTrace(message, args);

    public void LogWarning(string message, params object[] args) =>
        _logger.LogWarning(message, args);

    private static LogLevel Map(Core.LogLevel logLevel) => logLevel switch
    {
        Core.LogLevel.Trace => LogLevel.Trace,
        Core.LogLevel.Debug => LogLevel.Debug,
        Core.LogLevel.Information => LogLevel.Information,
        Core.LogLevel.Warning => LogLevel.Warning,
        Core.LogLevel.Error => LogLevel.Error,

        _ => throw new InvalidOperationException()
    };
}