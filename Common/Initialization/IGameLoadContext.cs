using Centuriin.CardGame.Core.Common.ECS.Entities.Players;
using Centuriin.CardGame.Core.Common.ECS.Templates;

namespace Centuriin.CardGame.Core.Common.Initialization;

public interface IGameLoadContext
{
    public IReadOnlyCollection<Player> Players { get; set; }

    public void AddTemplates<TTemplate>(IReadOnlyCollection<TTemplate> templates)
        where TTemplate : TemplateBase;

    public IReadOnlyCollection<TTemplate> GetTemplates<TTemplate>()
        where TTemplate : TemplateBase;
}
