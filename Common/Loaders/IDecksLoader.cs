namespace Centuriin.CardGame.Core.Common.Loaders;

public interface IDecksLoader
{
    public Task LoadAsync(GameTypeId gameTypeId, CancellationToken token);
}
