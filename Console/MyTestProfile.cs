using Centuriin.CardGame.Core.Common.GameProfiles;

namespace Centuriin.CardGame.Core.Console;

public sealed class MyTestProfile : IGameProfile
{
    public string Key => "TEST";

    public void Configure(IConfigurableGamePipeline pipeline)
    {
        ArgumentNullException.ThrowIfNull(pipeline);

        _ = pipeline
            .UseDefaultProfile();
    }
}
