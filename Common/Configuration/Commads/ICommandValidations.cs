using Centuriin.CardGame.Core.Common.Commands;
using Centuriin.CardGame.Core.Common.Commands.Rules;

namespace Centuriin.CardGame.Core.Common.Configuration.Commads;

internal interface ICommandValidations : IConfigurableCommandValidations
{
    public void Freeze();

    public void Apply(ICommandValidator validator, ICommandRuleFactory factory);
}
