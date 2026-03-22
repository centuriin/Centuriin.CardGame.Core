using Centuriin.CardGame.Core.Common.Engine;
using Centuriin.CardGame.Core.Common.Events;
using Centuriin.CardGame.Core.Common.Events.System;
using Centuriin.CardGame.Core.Common.Repositories;

namespace Centuriin.CardGame.Core.Drunkard;

public sealed class DecksLoader : IDecksLoader
{
    private readonly IEventDispatcher _dispatcher;
    private readonly ICardTemplateRepository _templateRepository;

    public DecksLoader(IEventDispatcher dispatcher, ICardTemplateRepository templateRepository)
    {
        ArgumentNullException.ThrowIfNull(dispatcher);
        _dispatcher = dispatcher;

        ArgumentNullException.ThrowIfNull(templateRepository);
        _templateRepository = templateRepository;
    }

    public async Task LoadDecksAsync(CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        var templateIds = await _templateRepository.GetTemplateIdsAsync(token);

        var gameId = Guid.CreateVersion7();
        var index = 0;
        foreach (var id in templateIds)
        {
            var @event = new CardInstanceCreated(
                new(gameId),
                new(index),
                id);

            await _dispatcher.PublishAsync(@event, token);

            index++;
        }
    }
}
