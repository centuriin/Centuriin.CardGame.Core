namespace Centuriin.CardGame.Core.Common;

public interface IClassicDealerService
{
    Task DealCardsAsync(int count, CancellationToken token);
}