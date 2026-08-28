using TCG.Model.Cards;
using TCG.Model.Core;
using TCG.Model.Events;
using UnityEngine;

namespace TCG.Model.Actions
{
    public class DestroyCardAction : MatchAction
    {
        private readonly int _toBeDestroyedCardId;
        private readonly int _toBeDestroyedSideIndexHint;

        public DestroyCardAction(int toBeDestroyedCardId, int toBeDestroyedCardSideIndexHint = 0)
        {
            _toBeDestroyedCardId = toBeDestroyedCardId;
            _toBeDestroyedSideIndexHint = toBeDestroyedCardSideIndexHint;
        }

        public override void Execute(Match match)
        {
            RuntimeCard card = match.FindRuntimeCardById(_toBeDestroyedCardId, _toBeDestroyedSideIndexHint);
            
            // fizzle checks
            if (card.State != RuntimeCard.CardState.OnBoard) return;
            
            Cell cell = match.GetCell(card.Position);

            cell.RemoveCard();
            card.SetState(RuntimeCard.CardState.InGraveyard);
            card.Owner.Graveyard.Add(card);
           
            
            match.EnqueueEvent( new MatchEvent()
            {
                Type = MatchEventType.CardDestroyed,
                SourceId = _toBeDestroyedCardId,
                TargetId = -1,
                Value = 0
            });
            Debug.Log($"Card {card.StaticCard.CardId} destroyed. Graveyard count: {card.Owner.Graveyard.Count}");
        }
    }
}