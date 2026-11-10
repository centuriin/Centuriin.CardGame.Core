namespace Centuriin.CardGame.Core.Engine.Loading;

public interface IGameLoader
{
    public Task LoadAsync(GameId gameId, CancellationToken token);
}