using Centuriin.CardGame.Core.Common.ECS.Entities.Players;
using Centuriin.CardGame.Core.Common.ECS.Templates;

namespace Centuriin.CardGame.Core.Common.Initialization;

public interface IReadOnlyGameLoadContext
{
    public IReadOnlyCollection<Player> Players { get; }

    public IReadOnlyCollection<TTemplate> GetTemplates<TTemplate>()
        where TTemplate : TemplateBase;
}
