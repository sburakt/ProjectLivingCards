using TCG.Model.Cards;
using TCG.Model.Core;
using TCG.Model.Events;
using UnityEngine;

namespace TCG.Model.Actions
{
    public class MoveCardAction : MatchAction
    {
        private readonly int _cardId;
        private readonly Position _targetPosition;

        public MoveCardAction(int cardId, int targetSideIndex, int targetLaneIndex, bool toFront)
        {
            _cardId = cardId;
            _targetPosition = new Position()
            {
                SideIndex = targetSideIndex,
                LaneIndex = targetLaneIndex,
                IsFront = toFront
            };
        }

        public MoveCardAction(int cardId, Position targetPosition)
        {
            _cardId = cardId;
            _targetPosition = targetPosition;
        }

        public override void Execute(Match match)
        {
            RuntimeCard card = match.FindRuntimeCardById(_cardId);
            // fizzle check card must be on the field for MoveACtion
            if (card == null || card.State != RuntimeCard.CardState.OnBoard) 
                return;

            Cell currentCell = match.GetCell(card.Position);
            Cell targetCell = match.GetCell(_targetPosition);
            
            // fizzle check card must move to an already empty cell
            if (targetCell.IsFull)
                return;
            // todo if this patter remove set update repeat i should make non action card moving in to match like match.MoveCard(..
            currentCell.RemoveCard();
            targetCell.SetCard(card);
            card.SetPosition(_targetPosition);
            
            // Event
            match.EnqueueEvent(new MatchEvent
            {
                Type = MatchEventType.CardMoved,
                SecondaryCardId = _cardId
            });
            
            Debug.Log($"ACTION: Moved Card {_cardId} to Lane {_targetPosition.LaneIndex} ({(_targetPosition.IsFront ? "Front" : "Back")})");
        }
    }
}