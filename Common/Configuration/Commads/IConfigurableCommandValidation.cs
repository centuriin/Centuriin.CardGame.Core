using Centuriin.CardGame.Core.Common.Commands.Rules;

namespace Centuriin.CardGame.Core.Common.Configuration.Commads;

public interface IConfigurableCommandValidation
{
    public IConfigurableCommandValidator CommandValidator { get; }

    public IConfigurableCommandValidation AddRule<TRule>()
        where TRule : IGameRule;
}
