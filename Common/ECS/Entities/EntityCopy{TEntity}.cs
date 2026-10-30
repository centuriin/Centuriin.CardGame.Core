namespace Centuriin.CardGame.Core.Common.ECS.Entities;

public sealed class EntityCopy<TEntity>
    where TEntity : EntityBase
{
    public TEntity Copy { get; }

    public EntityCopy(TEntity original)
    {
        ArgumentNullException.ThrowIfNull(original);
        Copy = (TEntity)original.Copy();
    }

    //public static implicit operator TEntity(EntityCopy<TEntity> source)
    //{
    //    ArgumentNullException.ThrowIfNull(source);

    //    return source.Copy;
    //}
}