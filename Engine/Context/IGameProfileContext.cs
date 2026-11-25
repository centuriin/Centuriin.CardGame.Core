using Centuriin.CardGame.Core.Engine.Loading.Configuration;

namespace Centuriin.CardGame.Core.Engine.Context;

/// <summary>
/// Provides the configuration profile selected for a game.
/// </summary>
public interface IGameProfileContext : IGameContext
{
    /// <summary>
    /// Gets the configuration profile selected for the game.
    /// </summary>
    public IGameProfile Profile { get; }
}