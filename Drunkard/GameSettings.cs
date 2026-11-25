using System.ComponentModel;

using Centuriin.CardGame.Core.Engine;

namespace Centuriin.CardGame.Core.Drunkard;

/// <summary>
/// Stores the deck selection for a Drunkard game.
/// </summary>
internal sealed class GameSettings : IGameSettings
{
    /// <summary>
    /// Gets the selected standard deck size.
    /// </summary>
    public DeckType DeckType { get; }

    /// <summary>
    /// Initializes a new <see cref="GameSettings"/> instance.
    /// </summary>
    /// <param name="deckType">
    /// The standard deck size to use.
    /// </param>
    public GameSettings(DeckType deckType)
    {
        if (!Enum.IsDefined(deckType) || deckType == default)
        {
            throw new InvalidEnumArgumentException(
                nameof(deckType),
                (int)deckType,
                typeof(DeckType));
        }
        DeckType = deckType;
    }

    /// <inheritdoc/>
    public bool Equals(IGameSettings? other) =>
        other is GameSettings settings
        && DeckType == settings.DeckType;
}
