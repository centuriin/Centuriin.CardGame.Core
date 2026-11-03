using Centuriin.CardGame.Core.Common.Context;
using Centuriin.CardGame.Core.Common.World;

namespace Centuriin.CardGame.Core.Common.Initialization;

/// <summary>
/// Describes game loader for game initialization before starting.
/// </summary>
public interface IGameEntityInitializer
{
    public Task InitializeAsync(
        GameSetup setup,
        IInitialStateContext initialStateContext,
        CancellationToken token);
}