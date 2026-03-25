namespace Centuriin.CardGame.Core.Common;

public interface IZonesFactory
{
    public Task CreateAsync(GameTypeId gameTypeId, CancellationToken token);
}