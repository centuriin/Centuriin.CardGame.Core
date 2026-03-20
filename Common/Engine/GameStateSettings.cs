using System;
using System.Collections.Generic;
using System.Text;

namespace Centuriin.CardGame.Core.Common.Engine;

public sealed class GameStateSettings
{
    public string GameType { get; set; }

    public int SpaceCount { get; set; }

    public int PlayerCount { get; set; }
}
