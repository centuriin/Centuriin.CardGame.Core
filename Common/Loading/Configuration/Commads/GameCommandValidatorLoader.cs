using Centuriin.CardGame.Core.Common.Commands;
using Centuriin.CardGame.Core.Common.Commands.Rules;
using Centuriin.CardGame.Core.Common.Context;

namespace Centuriin.CardGame.Core.Common.Loading.Configuration.Commads;

public sealed class GameCommandValidatorLoader : IGameLoader
{
    private readonly IGameContextAccessor<IGameProfileContext> _profileContextAccessor;
    private readonly ICommandRuleFactory _factory;
    private readonly ICommandValidator _validator;

    private readonly ConfigurableCommandValidator _configurableValidator = new();

    public GameCommandValidatorLoader(
        IGameContextAccessor<IGameProfileContext> profileContextAccessor,
        ICommandRuleFactory factory,
        ICommandValidator validator)
    {
        ArgumentNullException.ThrowIfNull(profileContextAccessor);
        _profileContextAccessor = profileContextAccessor;

        ArgumentNullException.ThrowIfNull(factory);
        _factory = factory;

        ArgumentNullException.ThrowIfNull(validator);
        _validator = validator;
    }

    /// <inheritdoc/>
    public async Task LoadAsync(GameId gameId, CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        var profile = (await _profileContextAccessor.GetContextAsync(gameId, token))
            .Profile;

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
