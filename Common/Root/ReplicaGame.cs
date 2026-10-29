using Centuriin.CardGame.Core.Common.Events.Application;
using Centuriin.CardGame.Core.Common.Events.Dispatching;
using Centuriin.CardGame.Core.Common.Events.Domain;
using Centuriin.CardGame.Core.Common.World;

namespace Centuriin.CardGame.Core.Common.Root;

public sealed class ReplicaGame : GameBase
{
    private readonly IMasterGameBridge _masterGameBridge;

    public ReplicaGame(
        GameId id, 
        GameTypeId typeId, 
        GameStatus status, 
        IGameState state, 
        IGameEventApplier eventApplier, 
        IAsyncDispatcher<IGameApplicationEvent> dispatcher,
        IMasterGameBridge masterGameBridge) 
        : base(id, typeId, status, state, eventApplier, dispatcher)
    {
        ArgumentNullException.ThrowIfNull(masterGameBridge);
        _masterGameBridge = masterGameBridge;
    }

    protected override async Task<IGameEventsBatch> StartCoreAsync(
        CancellationToken token)
        => await _masterGameBridge.ConnectAsync(token);

    protected override async Task<IGameEventsBatch> ApplyCoreAsync(
        IGameEvent @event,
        CancellationToken token)
        => await _masterGameBridge.SendAsync(@event, token);
    
    protected override async Task<IGameEventsBatch> AbortCoreAsync(
        CancellationToken token) 
        => await _masterGameBridge.DisconnectAsync(token);
}
