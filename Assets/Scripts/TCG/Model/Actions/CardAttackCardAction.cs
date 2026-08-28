using System;
using TCG.Model.Cards;
using TCG.Model.Core;
using TCG.Model.Events;
using UnityEngine;

namespace TCG.Model.Actions
{
    public class CardAttackCardAction : MatchAction
    {
        private readonly int _attackerSideIndex;
        private readonly int _attackerCardID;
        private readonly int _defenderCardID;

        public CardAttackCardAction(int attackerSideIndex, int attackerCardID, int defenderCardID)
        {
            _attackerSideIndex = attackerSideIndex;
            _attackerCardID = attackerCardID;
            _defenderCardID = defenderCardID;
        }

        public override void Execute(Match match)
        {
            Side defenderSide = match.Sides[_attackerSideIndex ^ 1];
            RuntimeCard attackerCard = match.FindRuntimeCardById(_attackerCardID, _attackerSideIndex);
            RuntimeCard defenderCard = match.FindRuntimeCardById(_defenderCardID, _attackerSideIndex ^ 1);

            //Fizzle checks

            // check if any card not on board
            
            if (attackerCard.State != RuntimeCard.CardState.OnBoard || defenderCard.State != RuntimeCard.CardState.OnBoard)
                return;

            // check if attacker card is on the attacker field
        
            if (attackerCard.Position.SideIndex != _attackerSideIndex)
                return;
            // check if defender card is on the defender field
            
            if (defenderCard.Position.SideIndex == _attackerSideIndex)
                return;

            // TODO check for any flag that cancels action ig cannott attack or cannot be attacked
          
            
            int amount = attackerCard.CalculateStat(match, RuntimeCard.StatType.Attack);
            match.PushAction( new DealCardDamageAction( _defenderCardID , _attackerSideIndex ^ 1, amount) );
            match.EnqueueEvent(new MatchEvent()
            {
                Type = MatchEventType.CardAttackCard,
                SourceId = _attackerCardID,
                TargetId = _defenderCardID
            });
        }
    }
}