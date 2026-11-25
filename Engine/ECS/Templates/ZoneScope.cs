namespace Centuriin.CardGame.Core.Engine.ECS.Templates;

/// <summary>
/// Controls how many zones are created from a zone template.
/// </summary>
public enum ZoneScope
{
    /// <summary>
    /// No value has been selected.
    /// </summary>
    None,
    /// <summary>
    /// Create one shared zone from the template.
    /// </summary>
    Singleton,
    /// <summary>
    /// Create one zone for each participant.
    /// </summary>
    PerPlayer
}
