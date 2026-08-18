using TCG.Model.Cards;
using TCG.Model.Core;
using TCG.Model.Events;
using UnityEngine;

namespace TCG.Model.Actions
{
    public class DrawCardAction : MatchAction
    {
        private int _drawingSideIndex;

        public DrawCardAction(int drawingSideIndex)
        {
            _drawingSideIndex = drawingSideIndex;
        }

        public override void Execute(Match match)
        {
            // get the drawing side and the deck
            Side drawingSide = match.Sides[_drawingSideIndex];
            // check if the deck is empty
            if (drawingSide.Deck.Count == 0) // should never reach below 0
            {
                Debug.Log($"Player {_drawingSideIndex} tried to draw, but deck is empty!");
                // event
                return;
            }

            // get the top card
            RuntimeCard drawnCard = drawingSide.Deck[drawingSide.Deck.Count - 1];
            // remove the top card from deck
            drawingSide.Deck.RemoveAt(drawingSide.Deck.Count - 1);
            // put the card to hand and change status
            drawingSide.Hand.Add(drawnCard);
            drawnCard.SetState(RuntimeCard.CardState.InHand);
            //debug
            string cardId = drawnCard.StaticCard.CardId;
            Debug.Log($"Side {_drawingSideIndex} drew: {cardId}. Hand size: {drawingSide.Hand.Count}");
            match.EnqueueEvent(new MatchEvent()
            {
                Type = MatchEventType.CardDrawn,
                SourceId = _drawingSideIndex,
                TargetId = drawnCard.InstanceId
            });
        }
    }
}