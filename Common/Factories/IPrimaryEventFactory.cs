namespace Centuriin.CardGame.Core.Common.Factories;

public interface IPrimaryEventFactory
{
    public TEvent Create<TEvent>();
}