using Centuriin.CardGame.Core.Common.ECS.Components;

namespace Centuriin.CardGame.Core.Common.ECS.Components.Zones;

public sealed record class HasPrimaryCards(int Count) : ComponentBase;