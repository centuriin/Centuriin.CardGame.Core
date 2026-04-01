namespace Centuriin.CardGame.Core.Common;

public interface IClassicDealerService
{
    public Task DealCardsAsync(int count, CancellationToken token);
}