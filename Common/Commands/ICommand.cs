using Centuriin.CardGame.Core.Common.Entities.Players;
using Centuriin.CardGame.Core.Common.World;

namespace Centuriin.CardGame.Core.Common.Commands;

public interface ICommand
{
    public GameId GameId { get; }

    public PlayerId Actor { get; }
}
