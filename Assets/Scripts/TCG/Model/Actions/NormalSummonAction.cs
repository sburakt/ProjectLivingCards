using TCG.Model.Cards;
using TCG.Model.Core;
using TCG.Model.Effects;
using TCG.Model.Events;
using UnityEngine;

namespace TCG.Model.Actions
{
    public class NormalSummonAction : MatchAction
    {
        private readonly int _cardId;
        private readonly Position _position;
        private readonly int _summonerSideIndex;

        public NormalSummonAction(int summonerSideIndex, int cardId, Position position)
        {
            _summonerSideIndex = summonerSideIndex;
            _cardId = cardId;
            _position = position;
        }

        public override void Execute(Match match)
        {
            RuntimeCard card = match.FindRuntimeCardById(_cardId, _summonerSideIndex);
            
            // fizzle checks
            if (card.State != RuntimeCard.CardState.InHand)
                return;

            Cell cell = match.GetCell(_position);
            SimpleSummonFromHand simpleSummonFromHand = new SimpleSummonFromHand(_cardId, _position);
            match.PushAction(simpleSummonFromHand);

            if (cell.IsFull)
            {
                VerticalPushAction.VerticalPushDirection pushDirection;
                if (_position.IsFront)
                    pushDirection = VerticalPushAction.VerticalPushDirection.Back;
                else
                    pushDirection = VerticalPushAction.VerticalPushDirection.Front;
                int toBePushedCardId = cell.Card.InstanceId;
                VerticalPushAction verticalPushAction = new VerticalPushAction(_cardId, toBePushedCardId, pushDirection);
                match.PushAction(verticalPushAction);
            }
            // event
            
            //Debug.Log(
            //     $"ACTION: Player {_summonerSideIndex} played {card.StaticCard.CardId} to Lane {_laneIndex} ({positionStr})");
        }
    }
}