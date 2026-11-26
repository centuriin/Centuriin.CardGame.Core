using Centuriin.CardGame.Core.Application.Modulation;
using Centuriin.CardGame.Core.Engine;
using Centuriin.CardGame.Core.Engine.Context;
using Centuriin.CardGame.Core.Engine.ECS.Entities;
using Centuriin.CardGame.Core.Engine.Root.State;

namespace Centuriin.CardGame.Core.Application.Initialization;

public sealed class GameInitializer : IGameInitializer
{
    private readonly IModulable<IEntityInitializer> _modulable;

    public GameInitializer(IModulable<IEntityInitializer> modulable)
    {
        ArgumentNullException.ThrowIfNull(modulable);
        _modulable = modulable;
    }

    /// <inheritdoc/>
    public async Task<IInitialStateContext> InitializeAsync(
        GameDescription description,
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(description);

        token.ThrowIfCancellationRequested();

        var context = new InitialStateContext()
        {
            GameId = description.GameId,
        };

        var entityInitializers = await _modulable.ResolveCollectionAsync(
            description.GameTypeId,
            token);

        foreach (var initializer in entityInitializers)
        {
            context.Entities.Add(
                await initializer.InitializeAsync(description, context, token));
        }

        return context;
    }

    /// <inheritdoc/>
    public async Task<IInitialStateContext> InitializeAsync(
        IGameSnapshot snapshot,
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(snapshot);

        token.ThrowIfCancellationRequested();

        return new InitialStateContext()
        {
            GameId = snapshot.GameId,
            Entities = new EntitiesCollection(snapshot.Entities)
        };
    }

    private sealed class InitialStateContext : IInitialStateContext
    {
        public required GameId GameId { get; init; }

        public EntityId LastEntityId => Entities.MaxBy(x => x.Id)?.Id
            ?? EntityId.Default;

        public IEntitiesCollection Entities { get; init; } = new EntitiesCollection();
    }
}
