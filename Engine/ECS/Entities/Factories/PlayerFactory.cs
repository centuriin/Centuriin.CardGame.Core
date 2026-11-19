using Centuriin.CardGame.Core.Engine.ECS.Entities.Players;
using Centuriin.CardGame.Core.Engine.ECS.Templates;

namespace Centuriin.CardGame.Core.Engine.ECS.Entities.Factories;

public sealed class PlayerFactory : EntityFactoryBase<Player, PlayerTemplate>
{
    public PlayerFactory(IEntityIdProvider entityIdProvider) 
        : base(entityIdProvider)
    {
    }

    protected override Player CreateEntity(EntityId id, PlayerTemplate template)
        => new(id, [.. template.Components]);
}
