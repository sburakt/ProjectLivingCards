using TCG.Model.Cards;
using TCG.Model.Core;
using TCG.Model.Effects;

namespace TCG.Model.Actions
{
    public class CardEnterFieldAction : MatchAction
    {
        private readonly int _cardId;
        
        public CardEnterFieldAction(int cardId)
        {
            _cardId = cardId;
        }
        public override void Execute(Match match)
        {
            RuntimeCard runtimeCard = match.FindRuntimeCardById(_cardId);

            foreach (Effect effect in runtimeCard.CardEffects)
            {
                match.AddEffect(effect);
            }
            // event 
            
        }
    }
}