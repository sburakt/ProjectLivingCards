using TCG.Model.Cards;
using TCG.Model.Core;
using TCG.Model.Effects;
using TCG.Model.Enums;
using TCG.Model.Events;
using UnityEngine;

namespace TCG.Model.Actions
{
    public class NormalSummonAction : MatchAction
    {
        private readonly int _cardId;
        private readonly Position _position;
        private readonly int _summonerSideIndex;

        public NormalSummonAction(int summonerSideIndex, int cardId, Position position, int groupId)
        {
            _summonerSideIndex = summonerSideIndex;
            _cardId = cardId;
            _position = position;
            GroupId = groupId;
        }

        public override void Execute(Match match)
        {
            base.UpdateGroupID(match);
            RuntimeCard card = match.FindRuntimeCardById(_cardId, _summonerSideIndex);
            
            // fizzle checks
            if (card.State != RuntimeCard.CardState.InHand)
                return;

            Cell cell = match.GetCell(_position);
            SimpleSummonFromHandAction simpleSummonFromHandAction = new SimpleSummonFromHandAction(_cardId, _position, GroupId);
            match.PushAction(simpleSummonFromHandAction);

            VerticalPushDirection pushDirection;
            if (_position.IsFront)
                pushDirection = VerticalPushDirection.Back;
            else
                pushDirection = VerticalPushDirection.Front;
            if (cell.IsFull)
            {
                int toBePushedCardId = cell.Card.InstanceId;
                VerticalPushAction verticalPushAction = new VerticalPushAction(_cardId, toBePushedCardId, pushDirection, GroupId);
                match.PushAction(verticalPushAction);
            }
            // event
            match.EnqueueEvent(new NormalSummonedEvent(_cardId, _summonerSideIndex, _position, pushDirection ,GroupId));
        }
    }
}