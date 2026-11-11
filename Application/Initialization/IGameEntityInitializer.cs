using Centuriin.CardGame.Core.Engine;
using Centuriin.CardGame.Core.Engine.Context;

namespace Application.Initialization;

/// <summary>
/// Describes game loader for game initialization before starting.
/// </summary>
public interface IGameEntityInitializer
{
    public Task InitializeAsync(
        GameDescription gameDescription,
        IInitialStateContext initialStateContext,
        CancellationToken token);
}