using Centuriin.CardGame.Core.Engine;

namespace Centuriin.CardGame.Core.Application;

public sealed class GameRequest
{
    public GameTypeId GameTypeId { get; }

    public PlayerId PlayerId { get; }

    public IGameSettings Settings { get; }

    public GameRequest(
        GameTypeId gameTypeId, 
        PlayerId playerId, 
        IGameSettings settings)
    {
        GameTypeId = gameTypeId;
        PlayerId = playerId;

        ArgumentNullException.ThrowIfNull(settings);
        Settings = settings;
    }
}
