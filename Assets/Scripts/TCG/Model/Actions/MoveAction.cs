using TCG.Model.Cards;
using TCG.Model.Core;
using TCG.Model.Events;
using UnityEngine;

namespace TCG.Model.Actions
{
    public class MoveCardAction : MatchAction
    {
        private readonly int _cardId;
        private readonly int _targetSideIndex;
        private readonly int _targetLaneIndex;
        private readonly bool _toFront;

        public MoveCardAction(int cardId, int targetSideIndex, int targetLaneIndex, bool toFront)
        {
            _cardId = cardId;
            _targetSideIndex = targetSideIndex;
            _targetLaneIndex = targetLaneIndex;
            _toFront = toFront;
        }

        public override void Execute(Match match)
        {
            RuntimeCard card = match.FindRuntimeCardById(_cardId);
            if (card == null || card.State != RuntimeCard.CardState.OnBoard) 
                return;

            Side side = match.Sides[card.Position.SideIndex];
            Lane oldLane = side.Field.Lanes[card.Position.LaneIndex];
            Lane newLane = match.Sides[_targetSideIndex].Field.Lanes[_targetLaneIndex];

            RuntimeCard targetPlaceCard = _toFront? newLane.FrontCard : newLane.BackCard; 
            
            // fizle check
            if (targetPlaceCard != null)
                return;

            
            if (card.Position.IsFront)
                oldLane.FrontCard = null;
            else
                oldLane.BackCard = null;

            
            if (_toFront)
                newLane.FrontCard = card;
            else
                newLane.BackCard = card;

            card.SetPosition(new Position()
            {
                SideIndex = card.Position.SideIndex,
                LaneIndex = _targetLaneIndex,
                IsFront = _toFront
            });

            match.EnqueueEvent(new MatchEvent
            {
                Type = MatchEventType.CardMoved,
                TargetId = _cardId
            });
            
            Debug.Log($"ACTION: Moved Card {_cardId} to Lane {_targetLaneIndex} ({(_toFront ? "Front" : "Back")})");
        }
    }
}