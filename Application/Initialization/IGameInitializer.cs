using Centuriin.CardGame.Core.Engine;
using Centuriin.CardGame.Core.Engine.Context;

namespace Centuriin.CardGame.Core.Application.Initialization;

/// <summary>
/// Describes game initializer for game initialization before starting.
/// </summary>
public interface IGameInitializer
{
    public Task InitializeAsync(
        GameDescription gameDescription,
        IInitialStateContext initialStateContext,
        CancellationToken token);
}