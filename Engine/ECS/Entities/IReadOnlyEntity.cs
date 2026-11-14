using Centuriin.CardGame.Core.Engine.ECS.Components;

namespace Centuriin.CardGame.Core.Engine.ECS.Entities;

public interface IReadOnlyEntity : IReadOnlyComponentsCollection
{
    public EntityId Id { get; }
}