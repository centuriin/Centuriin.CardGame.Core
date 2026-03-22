namespace Centuriin.CardGame.Core.Common.Engine;

public interface IDecksLoader
{
    public Task LoadDecksAsync(CancellationToken token);
}
