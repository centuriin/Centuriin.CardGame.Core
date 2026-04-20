using Centuriin.CardGame.Core.Common.Events;
using Centuriin.CardGame.Core.Common.GameProfiles;
using Centuriin.CardGame.Core.Common.Systems;

namespace Centuriin.CardGame.Core.Extensions.DependencyInjection;

internal sealed class MyTestProfile : IGameProfile
{
    public string Key => "TEST";

    public void Configure(IConfigurableGamePipeline pipeline)
    {
        ArgumentNullException.ThrowIfNull(pipeline);

        _ = pipeline
            .Add<SetupTurnFlowSystem, GameStartedEvent>()
            .Add<DealerSystem, GameStartedEvent>()
            .Add<TurnFlowSystem, TurnFlowDefinedEvent>()
            .Add<CardMovementSystem, CardDealtEvent>();
    }
}
