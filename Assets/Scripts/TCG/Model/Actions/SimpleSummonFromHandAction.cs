using TCG.Model.Cards;
using TCG.Model.Core;
using TCG.Model.Events;

namespace TCG.Model.Actions
{
    public class SimpleSummonFromHandAction : MatchAction
    {
        private readonly int _cardId;
        private readonly Position _position;

        public SimpleSummonFromHandAction(int cardId, Position position, int groupId)
        {
            _cardId = cardId;
            _position = position;
            GroupId = groupId;
        }
        public override void Execute(Match match)
        {
            base.UpdateGroupID(match);
            RuntimeCard card = match.FindRuntimeCardById(_cardId);
            Cell cell = match.GetCell(_position);

            // fizzle check
            if (card.State != RuntimeCard.CardState.InHand)
                return;
            if (cell.IsFull)
                return;

            Side side = card.Owner;
            side.Hand.Remove(card);
            card.SetState(RuntimeCard.CardState.OnBoard);
            cell.SetCard(card);
            card.SetPosition(_position);

            CardEnterFieldAction cardEnterFieldAction = new CardEnterFieldAction(_cardId, _position.SideIndex, GroupId);
            match.PushAction(cardEnterFieldAction);
            //event
            match.EnqueueEvent(new SimpleSummonedEvent(_cardId,_position,GroupId));

        }
    }
}