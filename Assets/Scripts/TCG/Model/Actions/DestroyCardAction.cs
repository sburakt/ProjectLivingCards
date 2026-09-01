using TCG.Model.Cards;
using TCG.Model.Core;
using TCG.Model.Enums;
using TCG.Model.Events;
using UnityEngine;

namespace TCG.Model.Actions
{
    public class DestroyCardAction : MatchAction
    {
        private readonly int _destroyerCardId;
        private readonly int _toBeDestroyedCardId;
        private readonly DestroyCause _destroyCause;

        public DestroyCardAction(int destroyerCardId, int toBeDestroyedCardId, DestroyCause destroyCause, int groupId)
        {
            _destroyerCardId = destroyerCardId;
            _toBeDestroyedCardId = toBeDestroyedCardId;
            _destroyCause = destroyCause;
            GroupId = groupId;
        }

        public override void Execute(Match match)
        {
            base.UpdateGroupID(match);
            RuntimeCard toBeDestroyedCard = match.FindRuntimeCardById(_toBeDestroyedCardId);
            
            // fizzle checks
            if (toBeDestroyedCard.State != RuntimeCard.CardState.OnBoard) return;
            
            Cell cell = match.GetCell(toBeDestroyedCard.Position);

            cell.RemoveCard();
            toBeDestroyedCard.SetState(RuntimeCard.CardState.InGraveyard);
            toBeDestroyedCard.Owner.Graveyard.Add(toBeDestroyedCard);
           
            
            match.EnqueueEvent( new DestroyedEvent(_destroyerCardId,_toBeDestroyedCardId, _destroyCause, GroupId));
        }
    }
}