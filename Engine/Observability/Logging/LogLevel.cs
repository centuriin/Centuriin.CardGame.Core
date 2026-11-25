namespace Centuriin.CardGame.Core.Engine.Observability.Logging;

/// <summary>
/// Defines the severity of an engine diagnostic message.
/// </summary>
public enum LogLevel
{
    /// <summary>
    /// The most detailed diagnostic messages.
    /// </summary>
    Trace = 0,

    /// <summary>
    /// Messages used to investigate engine behavior.
    /// </summary>
    Debug = 1,

    /// <summary>
    /// Messages describing normal engine operations.
    /// </summary>
    Information = 2,

    /// <summary>
    /// Messages describing unexpected conditions that may require attention.
    /// </summary>
    Warning = 3,

    /// <summary>
    /// Messages describing failed operations.
    /// </summary>
    Error = 4
}