namespace Centuriin.CardGame.Core.Common.ECS.Systems;

public interface ISystemFactory
{
    public SystemBase Create(Type systemType);
}