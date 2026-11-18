using Centuriin.CardGame.Core.Engine.ECS.Entities;

namespace Centuriin.CardGame.Core.Engine.ECS.Components;

/// <summary>
/// Owner component.
/// </summary>
/// <param name="CurrentOwnerId">
/// Current owner identifier.
/// </param>
public sealed record class OwnerComponent(EntityId CurrentOwnerId) : ComponentBase;