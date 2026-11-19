using Centuriin.CardGame.Core.Engine;
using Centuriin.CardGame.Core.Engine.Context;
using Centuriin.CardGame.Core.Engine.ECS.Entities.Factories;
using Centuriin.CardGame.Core.Engine.ECS.Entities.Players;
using Centuriin.CardGame.Core.Engine.ECS.Templates;

namespace Centuriin.CardGame.Core.Application.Initialization;

public sealed class PlayersInitializer : IGameInitializer
{
    private readonly IEntityFactory<Player, PlayerTemplate> _factory;

    public PlayersInitializer(IEntityFactory<Player, PlayerTemplate> factory)
    {
        ArgumentNullException.ThrowIfNull(factory);
        _factory = factory;
    }

    public Task InitializeAsync(
        GameDescription gameDescription,
        IInitialStateContext initialStateContext,
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(gameDescription);
        ArgumentNullException.ThrowIfNull(initialStateContext);

        token.ThrowIfCancellationRequested();

        foreach (var id in gameDescription.PlayerIds)
        {
            if (id == PlayerId.System)
            {
                initialStateContext.Entities.Add(Player.System);
                continue;
            }

            initialStateContext.Entities.Add(
                _factory.Create(PlayerTemplate.CreateDefault(id)));
        }

        return Task.CompletedTask;
    }
}
