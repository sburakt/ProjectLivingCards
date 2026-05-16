using TCG.Model.Cards;
using TCG.Model.Core;
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
            //todo The Fix: Zone Checking
            //  need to verify that the card is actually on the Field before trying to read its lane position
            // 
            RuntimeCard card = match.FindRuntimeCardById(_toBeDestroyedCardId, _toBeDestroyedSideIndexHint);
            // Safety check in case a null card or a card with no position data is passed
            if (card == null) return;
            if (card.State != RuntimeCard.CardState.OnBoard) return;
            // get the place  in the field
            Side ownerSide = card.Owner;
            Lane cardLane = ownerSide.Field.Lanes[card.Position.LaneIndex];

            // 1. Declare the target card in one line based on the position data
            RuntimeCard targetCard = card.Position.IsFront ? cardLane.FrontCard : cardLane.BackCard;

            // 2. Check if the card in that position is actually the card we are trying to destroy
            if (targetCard != card)
            {
                Debug.LogWarning("Attempted to destroy a card, but it was not in its expected position on the field.");
                return;
            }

            // 3. We confirmed it's the right card. Now remove it from the field.
            if (card.Position.IsFront)
                cardLane.FrontCard = null;
            else
                cardLane.BackCard = null;

            // 4. Clean up the card's state
            card.ChangeCurrentHealth(card.StaticCard.BaseHealth);
            card.ChangeCurrentAttack(card.StaticCard.BaseAttack);
            // card.Position = null; // Optional: Clear its position data since it's off the board!

            // 5. Send it to the graveyard
            ownerSide.Graveyard.Add(card);

            Debug.Log($"Card {card.StaticCard.CardId} destroyed. Graveyard count: {ownerSide.Graveyard.Count}");
        }
    }
}