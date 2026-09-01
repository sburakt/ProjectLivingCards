using TCG.Model.Cards;
using TCG.Model.Core;
using TCG.Model.Events;
using UnityEngine;

namespace TCG.Model.Actions
{
    public class DrawAction : MatchAction
    {
        private readonly int _drawingSideIndex;

        public DrawAction(int drawingSideIndex, int groupId)
        {
            _drawingSideIndex = drawingSideIndex;
            GroupId = groupId;
        }

        public override void Execute(Match match)
        {
            base.UpdateGroupID(match);
            // get the drawing side and the deck
            Side drawingSide = match.Sides[_drawingSideIndex];
            // check if the deck is empty
            if (drawingSide.Deck.Count == 0) // should never reach below 0
            {
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
            // events
            match.EnqueueEvent(new DrawnEvent(drawnCard.InstanceId,_drawingSideIndex,GroupId));
        }
    }
}