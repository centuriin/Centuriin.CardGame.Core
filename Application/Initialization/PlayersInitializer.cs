using Centuriin.CardGame.Core.Engine;
using Centuriin.CardGame.Core.Engine.Context;
using Centuriin.CardGame.Core.Engine.ECS.Entities;
using Centuriin.CardGame.Core.Engine.ECS.Entities.Factories;
using Centuriin.CardGame.Core.Engine.ECS.Entities.Players;
using Centuriin.CardGame.Core.Engine.ECS.Templates;

namespace Centuriin.CardGame.Core.Application.Initialization;

/// <summary>
/// Creates participant entities and the system player from a game description.
/// </summary>
public sealed class PlayersInitializer : IEntityInitializer
{
    private readonly IEntityFactory<Player, PlayerTemplate> _factory;

    /// <summary>
    /// Initializes a new <see cref="PlayersInitializer"/> instance.
    /// </summary>
    /// <param name="factory">
    /// The factory used to create the required entities or rules.
    /// </param>
    public PlayersInitializer(IEntityFactory<Player, PlayerTemplate> factory)
    {
        ArgumentNullException.ThrowIfNull(factory);
        _factory = factory;
    }

    /// <inheritdoc/>
    public Task<IReadOnlyCollection<EntityBase>> InitializeAsync(
        GameDescription description,
        IReadOnlyInitialStateContext context,
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(description);
        ArgumentNullException.ThrowIfNull(context);

        token.ThrowIfCancellationRequested();

        var result = new List<Player>(description.PlayerIds.Count);

        foreach (var id in description.PlayerIds)
        {
            if (id == PlayerId.System)
            {
                result.Add(Player.System);
                continue;
            }

            result.AddRange(
                _factory.Create(PlayerTemplate.CreateDefault(id))
                    .Select(x => x.Entity));
        }

        return Task.FromResult<IReadOnlyCollection<EntityBase>>(result);
    }
}
