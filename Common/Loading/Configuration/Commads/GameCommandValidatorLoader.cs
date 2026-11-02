using Centuriin.CardGame.Core.Common.Commands;
using Centuriin.CardGame.Core.Common.Commands.Rules;

namespace Centuriin.CardGame.Core.Common.Loading.Configuration.Commads;

public sealed class GameCommandValidatorLoader : IGameLoader
{
    private readonly ICommandRuleFactory _factory;
    private readonly ICommandValidator _validator;

    private readonly ConfigurableCommandValidator _configurableValidator = new();

    public GameCommandValidatorLoader(
        ICommandRuleFactory factory,
        ICommandValidator validator)
    {
        ArgumentNullException.ThrowIfNull(factory);
        _factory = factory;

        ArgumentNullException.ThrowIfNull(validator);
        _validator = validator;
    }

    /// <inheritdoc/>
    public Task LoadAsync(IGameProfile profile)
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
