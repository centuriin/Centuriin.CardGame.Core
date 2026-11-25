using Centuriin.CardGame.Core.Engine.ECS.Entities.Players;
using Centuriin.CardGame.Core.Engine.ECS.Templates;

namespace Centuriin.CardGame.Core.Engine.ECS.Entities.Factories;

/// <summary>
/// Creates player entities from templates with sequential identifiers.
/// </summary>
public sealed class PlayerFactory : EntityFactoryBase<Player, PlayerTemplate>
{
    /// <summary>
    /// Initializes a new <see cref="PlayerFactory"/> instance.
    /// </summary>
    /// <param name="entityIdProvider">
    /// The provider used to allocate entity identifiers.
    /// </param>
    public PlayerFactory(IEntityIdProvider entityIdProvider) 
        : base(entityIdProvider)
    {
    }

    /// <inheritdoc/>
    protected override Player CreateEntity(EntityId id, PlayerTemplate template)
        => new(id, [.. template.Components]);
}
