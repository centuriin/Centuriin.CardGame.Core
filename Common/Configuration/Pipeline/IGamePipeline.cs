using Centuriin.CardGame.Core.Common.ECS.Systems;
using Centuriin.CardGame.Core.Common.Events.Dispatching;
using Centuriin.CardGame.Core.Common.Events.Domain;

namespace Centuriin.CardGame.Core.Common.Configuration.Pipeline;

internal interface IGamePipeline : IConfigurableGamePipeline
{
    public void Freeze();

    public void Apply(IDispatcher<IGameEvent> dispatcher, ISystemFactory factory);
}