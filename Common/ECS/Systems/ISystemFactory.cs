namespace Centuriin.CardGame.Core.Common.ECS.Systems;

public interface ISystemFactory
{
    public TSystem Create<TSystem>() where TSystem : ISystem;
}