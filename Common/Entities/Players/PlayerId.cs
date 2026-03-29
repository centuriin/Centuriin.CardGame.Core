namespace Centuriin.CardGame.Core.Common.Entities.Players;

public readonly record struct PlayerId(Guid Value)
{
    public static PlayerId System { get; } = 
        new(Guid.Parse("00000000-0000-7000-8000-000000000000"));
}