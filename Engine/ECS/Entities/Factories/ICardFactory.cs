using Centuriin.CardGame.Core.Engine.ECS.Entities.Cards;
using Centuriin.CardGame.Core.Engine.ECS.Templates;

namespace Centuriin.CardGame.Core.Engine.ECS.Entities.Factories;

public interface ICardFactory
{
    public IReadOnlyCollection<Card> Create(
        IReadOnlyCollection<CardTemplate> templates);
}