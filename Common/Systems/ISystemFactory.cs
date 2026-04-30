namespace Centuriin.CardGame.Core.Common.Systems;

public interface ISystemFactory
{
    public SystemBase Create(Type systemType);
}