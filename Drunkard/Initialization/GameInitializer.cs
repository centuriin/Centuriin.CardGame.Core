using System;
using System.Collections.Generic;
using System.Text;

using Centuriin.CardGame.Core.Application.Initialization;
using Centuriin.CardGame.Core.Engine;
using Centuriin.CardGame.Core.Engine.Context;

namespace Centuriin.CardGame.Core.Drunkard.Initialization;

internal sealed class GameInitializer : IGameInitializer
{
    private readonly IEnumerable<IGameInitializer> _commonInitializers;

    public GameInitializer(IEnumerable<IGameInitializer> commonInitializers)
    {
        ArgumentNullException.ThrowIfNull(commonInitializers);
        _commonInitializers = commonInitializers;
    }

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
