using Centuriin.CardGame.Core.Engine;

namespace Centuriin.CardGame.Core.Application;

public interface IGameSettings : IEquatable<IGameSettings>
{
    public IReadOnlyList<PlayerId> PlayerIds { get; }
}