using System.Threading.Channels;

namespace Centuriin.CardGame.Core.Common.Events.Dispatching;

public sealed class GameEventBus : IGameEventBus
{
    private readonly Channel<IGameEvent> _channel = Channel.CreateUnbounded<IGameEvent>(new()
    {
        SingleReader = true,
        SingleWriter = true
    });

    public bool TryRead(out IGameEvent @event) => _channel.Reader.TryRead(out @event!);

    public void Write(IGameEvent @event) => _channel.Writer.TryWrite(@event);
}