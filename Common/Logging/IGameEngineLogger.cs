namespace Centuriin.CardGame.Core.Common.Logging;

public interface IGameEngineLogger
{
    public bool IsEnabled(LogLevel logLevel);

    public void LogError(string message, params object[] args);

    public void LogWarning(string message, params object[] args);

    public void LogInformation(string message, params object[] args);

    public void LogDebug(string message, params object[] args);

    public void LogTrace(string message, params object[] args);
}