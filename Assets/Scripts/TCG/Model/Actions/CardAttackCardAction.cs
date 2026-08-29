using System;
using TCG.Model.Cards;
using TCG.Model.Core;
using TCG.Model.Events;
using UnityEngine;

namespace TCG.Model.Actions
{
    public class CardAttackCardAction : MatchAction
    {
        private readonly int _attackerCardID;
        private readonly int _defenderCardID;

        public CardAttackCardAction(int attackerCardID, int defenderCardID)
        {
            _attackerCardID = attackerCardID;
            _defenderCardID = defenderCardID;
        }

        public override void Execute(Match match)
        {
            RuntimeCard attackerCard = match.FindRuntimeCardById(_attackerCardID);
            RuntimeCard defenderCard = match.FindRuntimeCardById(_defenderCardID);
            int attackerSideIndex = match.ActiveSideIndex;

            //Fizzle checks

            // check if any card not on board
            
            if (attackerCard.State != RuntimeCard.CardState.OnBoard || defenderCard.State != RuntimeCard.CardState.OnBoard)
                return;
            
            if (attackerCard.Position.SideIndex != attackerSideIndex)
                return;
            
            if (defenderCard.Position.SideIndex == attackerSideIndex)
                return;
            
            if (defenderCard.Position.LaneIndex != attackerCard.Position.LaneIndex)
                return;

            // TODO check for any flag that cancels action ig cannott attack or cannot be attacked
          
            
            int amount = attackerCard.CalculateStat(match, RuntimeCard.StatType.Attack);
            match.PushAction( new CardDamageCardAction( _attackerCardID , _defenderCardID, amount) );
            match.EnqueueEvent(new MatchEvent()
            {
                Type = MatchEventType.CardAttackedCard,
                PrimarySideIndex = attackerSideIndex,
                SecondarySideIndex = attackerSideIndex^1,
                PrimaryCardId = _attackerCardID,
                SecondaryCardId = _defenderCardID
            });
        }
    }
}