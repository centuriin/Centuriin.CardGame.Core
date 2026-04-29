namespace Centuriin.CardGame.Core.Common.Factories;

public interface IRuleFactory
{
    public TGameRule Create<TGameRule>();
}