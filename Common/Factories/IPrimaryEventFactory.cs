using Centuriin.CardGame.Core.Common.Events;

namespace Centuriin.CardGame.Core.Common.Factories;

public interface IPrimaryEventFactory
{
    public IPrimaryEvent Create();
}