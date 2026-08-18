using System;
using System.Collections;
using System.Collections.Generic;
using TCG.Model.Actions;
using TCG.Model.Cards;
using TCG.Model.Core;
using TCG.Model.Events;
using Unity.VisualScripting;

namespace TCG.Model.Actions
{
    public class DealCardDamageAction : MatchAction
    {
        private readonly int _targetCardId;
        private readonly int _targetSideIndex;
        private readonly int _amount;
        
        public DealCardDamageAction(int targetCardId, int targetSideIndex, int amount)
        {
            _targetCardId = targetCardId;
            _targetSideIndex = targetSideIndex;
            _amount = amount;
        }

        public override void Execute(Match match)
        {
            RuntimeCard target = match.FindRuntimeCardById(_targetCardId, _targetSideIndex);
            if (target == null || target.State != RuntimeCard.CardState.OnBoard)
                return;
            target.DecreaseHealth(_amount);
            if (target.CurrentHealth == 0) // im not sure about the damage checking the health for destruction here
                match.PushAction(new DestroyCardAction(_targetCardId, _targetSideIndex));
            match.EnqueueEvent(new MatchEvent()
            {
                Type = MatchEventType.CardDamaged
            });
        }
    }
}