using Centuriin.CardGame.Core.Common.Systems;

namespace Centuriin.CardGame.Core.Common.Factories;

public interface ISystemFactory
{
    public SystemBase Create(Type systemType);
}