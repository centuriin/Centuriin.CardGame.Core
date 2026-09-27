namespace Centuriin.CardGame.Core.Common.ECS.Entities.Players;

[Flags]
public enum PlayerRole
{
    None = 0,
    Bank = 1 << 0,
    Participant = 1 << 1
}