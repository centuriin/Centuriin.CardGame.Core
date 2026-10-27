using Centuriin.CardGame.Core.Common.Commands;
using Centuriin.CardGame.Core.Common.Commands.Rules;

namespace Centuriin.CardGame.Core.Common.Configuration.Commads;

public sealed class GameCommandValidatorInitializer : IGameConfigurationInitializer
{
    private readonly ICommandRuleFactory _factory;
    private readonly ICommandValidator _validator;

    private readonly ConfigurableCommandValidator _configurableValidator = new();

    public GameCommandValidatorInitializer(
        ICommandRuleFactory factory,
        ICommandValidator validator)
    {
        ArgumentNullException.ThrowIfNull(factory);
        _factory = factory;

        ArgumentNullException.ThrowIfNull(validator);
        _validator = validator;
    }

    /// <inheritdoc/>
    public void Initialize(IGameProfile profile)
    {
        ArgumentNullException.ThrowIfNull(profile);

        profile.Configure(_configurableValidator);

        var validationMap = _configurableValidator.Freeze();

        foreach (var (commandType, fabric) in validationMap)
        {
            _validator.AddValidation(
                commandType,
                [.. fabric.Select(x => x.Invoke(_factory))]);
        }
    }
}
