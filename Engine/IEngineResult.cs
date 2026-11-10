using Centuriin.CardGame.Core.Engine.Root.State;

namespace Centuriin.CardGame.Core.Engine;

public interface IEngineResult
{
    public GameVersion GameVersion { get; }
}
