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
            // Safety checks and fizzle
            if (card == null) return;
            if (card.State != RuntimeCard.CardState.OnBoard) return;
            // get the place  in the field
            Side ownerSide = card.Owner;
            Lane cardLane = ownerSide.Field.Lanes[card.Position.LaneIndex];

            RuntimeCard targetCard = card.Position.IsFront ? cardLane.FrontCard : cardLane.BackCard;

            if (targetCard != card)
            {
                Debug.LogWarning("Attempted to destroy a card, but it was not in its expected position on the field.");
                return;
            }

            if (card.Position.IsFront)
                cardLane.FrontCard = null;
            else
                cardLane.BackCard = null;

            // clean up handle should be done systematically not in the destruction refactor this later
            // maybe send graveyard as an action not sure if that will be needed any effect that will work between destroy and graveyard possible

            card.SetState(RuntimeCard.CardState.InGraveyard);
            ownerSide.Graveyard.Add(card);
            
            match.EnqueueEvent( new MatchEvent()
            {
                Type = MatchEventType.CardDestroyed,
                SourceId = _toBeDestroyedCardId,
                TargetId = -1,
                Value = 0
            });
            Debug.Log($"Card {card.StaticCard.CardId} destroyed. Graveyard count: {ownerSide.Graveyard.Count}");
        }
    }
}