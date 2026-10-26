using Centuriin.CardGame.Core.Common.Configuration.Commads;
using Centuriin.CardGame.Core.Common.Configuration.Pipeline;

namespace Centuriin.CardGame.Core.Common.Configuration;

internal sealed class GameConfiguration : IGameConfiguration
{
    private readonly IGamePipeline _pipeline;
    private readonly ICommandValidations _commandValidations;

    internal GameConfiguration(
        IGamePipeline pipeline,
        ICommandValidations commandValidations)
    {
        ArgumentNullException.ThrowIfNull(pipeline);
        _pipeline = pipeline;

        ArgumentNullException.ThrowIfNull(commandValidations);
        _commandValidations = commandValidations;
    }

    public void Apply(IGameConfigurationContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        _pipeline.Apply(context.Dispatcher, context.SystemFactory);

        _commandValidations.Apply(context.CommandValidator, context.RuleFactory);
    }
}
