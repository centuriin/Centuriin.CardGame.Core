namespace Centuriin.CardGame.Core.Engine.ECS.Components.Zones;

/// <summary>
/// Specifies how many cards a zone receives during the initial deal.
/// </summary>
/// <param name="Count">
/// The number of cards dealt to the zone at game startup.
/// </param>
public sealed record class HasPrimaryCards(int Count) : ComponentBase;