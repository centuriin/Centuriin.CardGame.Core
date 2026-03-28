namespace Centuriin.CardGame.Core.Common;

public interface IDecksLoader
{
    public Task LoadAsync(GameTypeId gameTypeId, CancellationToken token);
}
