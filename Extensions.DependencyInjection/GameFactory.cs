using Centuriin.CardGame.Core.Common.Factories;
using Centuriin.CardGame.Core.Common.World;

using Microsoft.Extensions.DependencyInjection;

namespace Centuriin.CardGame.Core.Extensions.DependencyInjection;

public sealed class GameFactory : IGameFactory
{
    private readonly IServiceProvider _serviceProvider;

    public GameFactory(IServiceProvider serviceProvider)
    {
        ArgumentNullException.ThrowIfNull(serviceProvider);
        _serviceProvider = serviceProvider;
    }

    public IGame Create() =>
        ActivatorUtilities.CreateInstance<ServerGame>(
            _serviceProvider, 
            new GameId(Guid.CreateVersion7()), 
            GameStatus.Pending);

    public IGame Create(IGameSnapshot snapshot) =>
        ActivatorUtilities.CreateInstance<ServerGame>(
            _serviceProvider, 
            snapshot.GameStatus, 
            snapshot.GameState,
            snapshot.Version);
}
