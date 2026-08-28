using TCG.Model.Cards;
using TCG.Model.Core;

namespace TCG.Model.Actions
{
    public class SimpleSummonFromHand : MatchAction
    {
        private readonly int _cardId;
        private readonly Position _position;

        public SimpleSummonFromHand(int cardId, Position position)
        {
            _cardId = cardId;
            _position = position;
        }
        public override void Execute(Match match)
        {
            
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

            CardEnterFieldAction cardEnterFieldAction = new CardEnterFieldAction(_cardId);
            match.PushAction(cardEnterFieldAction);
            //event

        }
    }
}