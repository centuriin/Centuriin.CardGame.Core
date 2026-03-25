using Centuriin.CardGame.Core.Common;
using Centuriin.CardGame.Core.Common.Events;
using Centuriin.CardGame.Core.Common.Events.System;
using Centuriin.CardGame.Core.Common.Repositories;

namespace Centuriin.CardGame.Core.Drunkard;

public sealed class DecksLoader : IDecksLoader
{
    private readonly ICardTemplatesRepository _templateRepository;

    public DecksLoader(ICardTemplatesRepository templateRepository)
    {
        ArgumentNullException.ThrowIfNull(templateRepository);
        _templateRepository = templateRepository;
    }

    public async Task LoadAsync(CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        var templateIds = await _templateRepository.GetTemplateIdsByGameTypeAsync(
            default,
            token);

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
