using System;
using TCG.Model.Cards;
using TCG.Model.Core;
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

            // check if any card is null

            // check if attacker card is on the attacker field

            // check if defender card is on the defender field

            // check for any flag that cancels action ig cannott attack or cannot be attacked
            
            int amount = attackerCard.CalculateStat(match, RuntimeCard.StatType.Attack);
            match.PushAction( new DealCardDamageAction( _defenderCardID , _attackerSideIndex ^ 1, amount) );
        }
    }
}