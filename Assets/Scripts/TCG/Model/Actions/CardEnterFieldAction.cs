using System.Text.RegularExpressions;
using TCG.Model.Cards;
using TCG.Model.Effects;
using TCG.Model.Events;
using Match = TCG.Model.Core.Match;

namespace TCG.Model.Actions
{
    public class CardEnterFieldAction : MatchAction
    {
        private readonly int _cardId;
        private readonly int _enteredSideIndex;
        
        public CardEnterFieldAction(int cardId, int enteredSideIndex, int groupId)
        {
            _cardId = cardId;
            _enteredSideIndex = enteredSideIndex;
            GroupId = groupId;
        }
        public override void Execute(Match match)
        {
            base.UpdateGroupID(match);
            RuntimeCard runtimeCard = match.FindRuntimeCardById(_cardId);

            foreach (Effect effect in runtimeCard.CardEffects)
            {
                match.AddEffect(effect);
            }
            // event 
            match.EnqueueEvent(new EnteredFieldEvent(_cardId, _enteredSideIndex, GroupId));
            
        }
    }
}