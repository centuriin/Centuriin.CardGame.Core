using Centuriin.CardGame.Core.Common.Root;
using Centuriin.CardGame.Core.Common.Root.State;

namespace Centuriin.CardGame.Core.Common.Context;

public interface IGameContext
{
    public GameId GameId { get; }

    public GameVersion GameVersion { get; }

    public void ThrowIfNotApplicable(IGame game);
}