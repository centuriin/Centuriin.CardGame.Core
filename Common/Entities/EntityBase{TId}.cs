namespace Centuriin.CardGame.Core.Common.Entities;

public abstract class EntityBase<TId> : EntityBase
    where TId : struct, IEquatable<TId>
{
    /// <summary>
    /// Instance id.
    /// </summary>
    public TId Id { get; }

    protected EntityBase(TId id)
    {
        Id = id;
    }
}
