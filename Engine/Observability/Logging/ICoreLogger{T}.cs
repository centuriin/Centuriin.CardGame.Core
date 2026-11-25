namespace Centuriin.CardGame.Core.Engine.Observability.Logging;

/// <summary>
/// Writes engine diagnostic messages without depending on a logging framework.
/// </summary>
/// <typeparam name="T">
/// The type used as the logging category.
/// </typeparam>
public interface ICoreLogger<T> : ICoreLogger;