namespace Centuriin.CardGame.Core.Common.Context;

public interface IGameContext
{
    public GameDescription GameDescription { get; }

    public void ThrowIfNotApplicable(GameDescription gameDescription);
}