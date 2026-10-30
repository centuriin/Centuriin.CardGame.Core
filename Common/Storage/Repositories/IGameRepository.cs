using System;
using System.Collections.Generic;
using System.Text;

using Centuriin.CardGame.Core.Common.Root.State;

namespace Centuriin.CardGame.Core.Common.Storage.Repositories;

public interface IGameRepository
{
    public Task UpdateVersion(GameId gameId, GameVersion newVersion);
}
