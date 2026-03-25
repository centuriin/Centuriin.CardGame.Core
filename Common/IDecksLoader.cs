namespace Centuriin.CardGame.Core.Common;

public interface IDecksLoader
{
    public Task LoadAsync(CancellationToken token);
}
