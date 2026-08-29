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
    public class CardDamageCardAction : MatchAction
    {
        private readonly int _sourceCardID;
        private readonly int _targetCardId;
        private readonly int _amount;
        
        public CardDamageCardAction(int sourceCardID, int targetCardId, int amount)
        {
            _sourceCardID = sourceCardID;
            _targetCardId = targetCardId;
            _amount = amount;
        }

        public override void Execute(Match match)
        {
            RuntimeCard target = match.FindRuntimeCardById(_targetCardId);
            
            // fizzle check
            if (target.State != RuntimeCard.CardState.OnBoard)
                return;
            
            target.DecreaseHealth(_amount);
            
            if (target.CurrentHealth == 0) // im not sure about the damage checking the health for destruction here
                match.PushAction(new CardDestroyCardAction(_sourceCardID, _targetCardId));
            match.EnqueueEvent(new MatchEvent()
            {
                Type = MatchEventType.CardDamagedCard,
                PrimaryCardId = _sourceCardID,
                SecondaryCardId = _targetCardId,
                Value = _amount
            });
        }
    }
}