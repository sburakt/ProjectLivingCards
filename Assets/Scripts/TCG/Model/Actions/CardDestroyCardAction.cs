using TCG.Model.Cards;
using TCG.Model.Core;
using TCG.Model.Events;
using UnityEngine;

namespace TCG.Model.Actions
{
    public class CardDestroyCardAction : MatchAction
    {
        private readonly int _destroyerCardId;
        private readonly int _toBeDestroyedCardId;

        public CardDestroyCardAction(int destroyerCardId, int toBeDestroyedCardId)
        {
            _destroyerCardId = destroyerCardId;
            _toBeDestroyedCardId = toBeDestroyedCardId;
        }

        public override void Execute(Match match)
        {
            RuntimeCard toBeDestroyedCard = match.FindRuntimeCardById(_toBeDestroyedCardId);
            
            // fizzle checks
            if (toBeDestroyedCard.State != RuntimeCard.CardState.OnBoard) return;
            
            Cell cell = match.GetCell(toBeDestroyedCard.Position);

            cell.RemoveCard();
            toBeDestroyedCard.SetState(RuntimeCard.CardState.InGraveyard);
            toBeDestroyedCard.Owner.Graveyard.Add(toBeDestroyedCard);
           
            
            match.EnqueueEvent( new MatchEvent()
            {
                Type = MatchEventType.CardDestroyedCard,
                PrimaryCardId = _destroyerCardId,
                SecondaryCardId = _toBeDestroyedCardId,
            });
            Debug.Log($"Card {_destroyerCardId} destroyed {toBeDestroyedCard.StaticCard.CardId}.");
        }
    }
}