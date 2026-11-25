using Centuriin.CardGame.Core.Application.Initialization;
using Centuriin.CardGame.Core.Engine;
using Centuriin.CardGame.Core.Engine.Context;

namespace Centuriin.CardGame.Core.Drunkard.Initialization;

/// <summary>
/// Runs the common entity initializers for the Drunkard module.
/// </summary>
internal sealed class GameInitializer : IGameInitializer
{
    private readonly IEnumerable<IGameInitializer> _commonInitializers;

    /// <summary>
    /// Initializes a new <see cref="GameInitializer"/> instance.
    /// </summary>
    /// <param name="commonInitializers">
    /// The common initializers in execution order.
    /// </param>
    public GameInitializer(IEnumerable<IGameInitializer> commonInitializers)
    {
        ArgumentNullException.ThrowIfNull(commonInitializers);
        _commonInitializers = commonInitializers;
    }

    /// <inheritdoc/>
    public async Task InitializeAsync(
        GameDescription gameDescription,
        IInitialStateContext initialStateContext,
        CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(gameDescription);
        ArgumentNullException.ThrowIfNull(initialStateContext);

        token.ThrowIfCancellationRequested();

        foreach (var initializer in _commonInitializers)
        {
            await initializer.InitializeAsync(gameDescription, initialStateContext, token);
        }
    }
}
