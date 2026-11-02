namespace Centuriin.CardGame.Core.Common.Loading;

public interface IGameLoader
{
    public Task LoadAsync(GameId gameId, CancellationToken token);
}