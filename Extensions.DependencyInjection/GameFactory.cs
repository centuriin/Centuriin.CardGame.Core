using Centuriin.CardGame.Core.Common;
using Centuriin.CardGame.Core.Common.Root;
using Centuriin.CardGame.Core.Common.Root.State;
using Centuriin.CardGame.Core.Common.World;

using Microsoft.Extensions.DependencyInjection;

namespace Centuriin.CardGame.Core.Extensions.DependencyInjection;

internal sealed class GameFactory : IGameFactory
{
    private readonly IServiceProvider _serviceProvider;

    public GameFactory(IServiceProvider serviceProvider)
    {
        ArgumentNullException.ThrowIfNull(serviceProvider);
        _serviceProvider = serviceProvider;
    }

    public TGame Create<TGame>(GameTypeId typeId) 
        where TGame : IGame =>
        ActivatorUtilities.CreateInstance<TGame>(
            _serviceProvider, 
            new GameId(Guid.CreateVersion7()), 
            typeId,
            GameStatus.Created);

    public TGame Create<TGame>(IGameSnapshot snapshot) 
        where TGame : IGame =>
        ActivatorUtilities.CreateInstance<TGame>(
            _serviceProvider, 
            snapshot.GameId,
            snapshot.TypeId,
            snapshot.GameStatus, 
            snapshot.GameState,
            snapshot.Version);
}
