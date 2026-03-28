namespace Centuriin.CardGame.Core.Common.Entities.Players;

public enum PlayerRole
{
    None = 0,
    Bank = 1 << 0,
    Participant = 1 << 1
}