namespace Centuriin.CardGame.Core.Common.Context;

public abstract class GameContextBase : IGameContext
{
    public GameDescription GameDescription { get; }

    protected GameContextBase(GameDescription gameDescription)
    {
        ArgumentNullException.ThrowIfNull(gameDescription);
        GameDescription = gameDescription;
    }

    public void ThrowIfNotApplicable(GameDescription gameDescription)
    {
        ArgumentNullException.ThrowIfNull(gameDescription);

        if (GameDescription != gameDescription)
        {
            throw new InvalidOperationException(
                $"Game context is not applicable to this game description." +
                $" Context game-version is:" +
                $" {GameDescription.GameId.Value}-{GameDescription.GameVersion.Value}," +
                $" but description game-version is:" +
                $" {gameDescription.GameId.Value}-{gameDescription.GameVersion.Value}");
        }
    }
}
