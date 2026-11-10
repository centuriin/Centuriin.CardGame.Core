namespace Centuriin.CardGame.Core.Engine.ECS.Systems;

public interface ISystemFactory
{
    public TSystem Create<TSystem>() where TSystem : ISystem;
}