using Centuriin.CardGame.Core.Common.Engine;
using Centuriin.CardGame.Core.Common.Events;
using Centuriin.CardGame.Core.Common.Events.System;
using Centuriin.CardGame.Core.Common.Repositories;

namespace Centuriin.CardGame.Core.Drunkard;

public sealed class DecksLoader : IDecksLoader
{
    private readonly ICardTemplateRepository _templateRepository;

    public DecksLoader(ICardTemplateRepository templateRepository)
    {
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

            //await _dispatcher.PublishAsync(@event, token);

            index++;
        }
    }
}
