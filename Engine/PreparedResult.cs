using Centuriin.CardGame.Core.Engine.Root.State;

namespace Centuriin.CardGame.Core.Engine;

public sealed record class PreparedResult(GameVersion GameVersion) : IEngineResult;