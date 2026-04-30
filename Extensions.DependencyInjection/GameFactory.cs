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

    public TGame Create<TGame>() where TGame : IGame =>
        ActivatorUtilities.CreateInstance<TGame>(
            _serviceProvider, 
            new GameId(Guid.CreateVersion7()), 
            GameStatus.Pending);

    public TGame Create<TGame>(IGameSnapshot snapshot) where TGame : IGame =>
        ActivatorUtilities.CreateInstance<TGame>(
            _serviceProvider, 
            snapshot.GameStatus, 
            snapshot.GameState,
            snapshot.Version);
}
