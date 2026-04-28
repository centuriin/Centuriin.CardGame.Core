namespace Centuriin.CardGame.Core.Common.Commands;

public sealed class GameRulesBuilder : IGameRulesBuilder, IConfigurableGameRulesBuilder
{
    private readonly Dictionary<Type, List<Type>> _ruleMappings = [];
    private readonly IRuleFactory _factory;

    public GameRulesBuilder(IRuleFactory factory)
    {
        _factory = factory;
    }

    public IConfigurableGameRulesBuilder AddRule<TCommand, TRule>()
        where TCommand : ICommand
        where TRule : IGameRule
    {
        var commandType = typeof(TCommand);

        if (!_ruleMappings.TryGetValue(commandType, out var rules))
        {
            rules = [];
            _ruleMappings[commandType] = rules;
        }

        rules.Add(typeof(TRule));

        return this;
    }

    public ICommandValidator Build() => throw new NotImplementedException();
}
