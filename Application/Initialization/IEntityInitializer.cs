using Centuriin.CardGame.Core.Engine;
using Centuriin.CardGame.Core.Engine.Context;
using Centuriin.CardGame.Core.Engine.ECS.Entities;

namespace Centuriin.CardGame.Core.Application.Initialization;

public interface IEntityInitializer
{
    public Task<IReadOnlyCollection<EntityBase>> InitializeAsync(
        GameDescription description,
        IReadOnlyInitialStateContext context,
        CancellationToken token);
}
