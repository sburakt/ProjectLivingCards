using System;
using System.Collections;
using System.Collections.Generic;
using TCG.Model.Actions;
using TCG.Model.Cards;
using TCG.Model.Core;
using TCG.Model.Enums;
using TCG.Model.Events;
using Unity.VisualScripting;

namespace TCG.Model.Actions
{
    public class DamageCardAction : MatchAction
    {
        private readonly int _sourceCardID;
        private readonly int _targetCardId;
        private readonly int _amount;
        private readonly DamageType _damageType;
        
        
        public DamageCardAction(int sourceCardID, int targetCardId, int amount, DamageType damageType, int groupId)
        {
            _sourceCardID = sourceCardID;
            _targetCardId = targetCardId;
            _amount = amount;
            _damageType = damageType;
            GroupId = groupId;
        }

        public override void Execute(Match match)
        {
            base.UpdateGroupID(match);
            RuntimeCard target = match.FindRuntimeCardById(_targetCardId);
            
            // fizzle check
            if (target.State != RuntimeCard.CardState.OnBoard)
                return;
            
            target.DecreaseHealth(_amount);

            if (target.CurrentHealth == 0) // im not sure about the damage checking the health for destruction here
            {
                DestroyCause destroyCause = _damageType == DamageType.AttackDamage
                    ? DestroyCause.AttackDamage
                    : DestroyCause.EffectDamage;
                match.PushAction(new DestroyCardAction(_sourceCardID, _targetCardId, destroyCause, GroupId));
            }

            match.EnqueueEvent(new DamagedCardEvent(_sourceCardID, _targetCardId, _amount, _damageType, GroupId));
        }
    }
}