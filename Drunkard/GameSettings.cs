using System.ComponentModel;

using Centuriin.CardGame.Core.Application;

namespace Centuriin.CardGame.Core.Drunkard;

internal sealed class GameSettings : IGameSettings
{
    public DeckType DeckType { get; }

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

    public bool Equals(IGameSettings? other) =>
        other is GameSettings settings
        && DeckType == settings.DeckType;
}
