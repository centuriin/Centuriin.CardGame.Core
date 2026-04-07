namespace Centuriin.CardGame.Core.Common.Loaders;

/// <summary>
/// Describes game loader for game initialization before starting.
/// </summary>
public interface IGameLoader
{
    public Task LoadAsync(GameSetup setup, IGameState gameState, CancellationToken token);
}