using Centuriin.CardGame.Core.Engine;

namespace Centuriin.CardGame.Core.Application;

public interface IGameModuleAdapter<TDependency>
    where TDependency : notnull
{
    public ValueTask<TDependency> GetDependencyAsync(
        GameTypeId gameTypeId,
        CancellationToken token);
}