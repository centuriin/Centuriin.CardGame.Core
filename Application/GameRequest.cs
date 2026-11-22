using Centuriin.CardGame.Core.Engine;

namespace Centuriin.CardGame.Core.Application;

public sealed class GameRequest<TGameMarker>
    where TGameMarker : IGameMarker
{
    public GameTypeId GameTypeId { get; }

    public PlayerId PlayerId { get; }

    public IGameSettings<TGameMarker> Settings { get; }

    public GameRequest(
        GameTypeId gameTypeId, 
        PlayerId playerId, 
        IGameSettings<TGameMarker> settings)
    {
        GameTypeId = gameTypeId;
        PlayerId = playerId;

        ArgumentNullException.ThrowIfNull(settings);
        Settings = settings;
    }
}
