using Microsoft.Extensions.Logging;

using Engine = Centuriin.CardGame.Core.Common.Observability.Logging;

namespace Centuriin.CardGame.Core.Extensions.DependencyInjection;

internal sealed class CoreLoggerAdapter<T> : Engine.ICoreLogger<T>
{
    private readonly ILogger<T> _logger;

    public CoreLoggerAdapter(ILogger<T> logger)
    {
        ArgumentNullException.ThrowIfNull(logger);
        _logger = logger;
    }

    public bool IsEnabled(Engine.LogLevel logLevel) => _logger.IsEnabled(Map(logLevel));

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

    private static LogLevel Map(Engine.LogLevel logLevel) => logLevel switch
    {
        Engine.LogLevel.Trace => LogLevel.Trace,
        Engine.LogLevel.Debug => LogLevel.Debug,
        Engine.LogLevel.Information => LogLevel.Information,
        Engine.LogLevel.Warning => LogLevel.Warning,
        Engine.LogLevel.Error => LogLevel.Error,

        _ => throw new InvalidOperationException()
    };
}