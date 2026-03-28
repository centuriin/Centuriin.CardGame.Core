namespace Centuriin.CardGame.Core.Common.Loaders;

public interface IZonesLoader
{
    public Task LoadAsync(GameTypeId gameTypeId, CancellationToken token);
}