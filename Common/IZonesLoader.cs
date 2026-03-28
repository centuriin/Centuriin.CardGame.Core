namespace Centuriin.CardGame.Core.Common;

public interface IZonesLoader
{
    public Task LoadAsync(GameTypeId gameTypeId, CancellationToken token);
}