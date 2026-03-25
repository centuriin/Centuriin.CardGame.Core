namespace Centuriin.CardGame.Core.Common.Components;

public sealed record class AllowedTransitionComponent<TFromId> : ComponentBase
    where TFromId : IEquatable<TFromId>
{
    public HashSet<TFromId> AllowedFrom { get; }

    public AllowedTransitionComponent(HashSet<TFromId> allowedFrom)
    {
        ArgumentNullException.ThrowIfNull(allowedFrom);
        AllowedFrom = allowedFrom;
    }

    private AllowedTransitionComponent(AllowedTransitionComponent<TFromId> other)
        : base(other)
    {
        AllowedFrom = other.AllowedFrom.ToHashSet();
    }
}
