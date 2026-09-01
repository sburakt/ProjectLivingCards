using TCG.Model.Cards;
using TCG.Model.Core;
using TCG.Model.Events;

namespace TCG.Model.Actions
{
    public class MoveCardAction : MatchAction
    {
        private readonly int _cardId;
        private readonly Position _targetPosition;

        public MoveCardAction(int cardId, Position targetPosition, int groupId)
        {
            _cardId = cardId;
            _targetPosition = targetPosition;
            GroupId = groupId;
        }

        public override void Execute(Match match)
        {
            base.UpdateGroupID(match);
            RuntimeCard card = match.FindRuntimeCardById(_cardId);
            // fizzle check card must be on the field for MoveACtion
            if (card.State != RuntimeCard.CardState.OnBoard) 
                return;

            Position oldPosition = card.Position;
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
            match.EnqueueEvent(new MovedEvent(_cardId, oldPosition, _targetPosition, GroupId));
            
        }
    }
}