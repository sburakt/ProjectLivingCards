using TCG.Model.Cards;
using TCG.Model.Core;
using TCG.Model.Events;
using UnityEngine;

namespace TCG.Model.Actions
{
    public class CardAttackSideAction : MatchAction
    {
        private readonly int _attackerCardID;
        private readonly int _defenderSideIndex;

        public CardAttackSideAction( int attackerCardID ,int defenderSideIndex)
        {
            _attackerCardID = attackerCardID;
            _defenderSideIndex = defenderSideIndex;
        }

        public override void Execute(Match match)
        {
            RuntimeCard attackerCard = match.FindRuntimeCardById(_attackerCardID);
            Side defenderSide = match.Sides[_defenderSideIndex];
            
            int damage = attackerCard.CalculateStat(match, RuntimeCard.StatType.Attack);

            //Fizzle checks

            // check if attacker card is on field
            if(attackerCard.State != RuntimeCard.CardState.OnBoard)
                return;
            // check if attacker card is on the attacker field
            if(attackerCard.Position.SideIndex == _defenderSideIndex)
                return;

            // check for any flag that cancels action ig cannot attack or no direct attack
            
            defenderSide.LifePoints -= damage;
           
            if (damage > 0)
                match.EnqueueEvent(new MatchEvent()
                {
                    Type = MatchEventType.CardDamagedSide,
                    PrimaryCardId = _attackerCardID,
                    SecondarySideIndex = _defenderSideIndex,
                    Value = damage
                });

            // check for game over condition
            if (defenderSide.LifePoints <= 0)
            {
                // prevent health from showing as a negative number in the view
                defenderSide.LifePoints = 0;
                // todo game over logic
            }
            // event
            match.EnqueueEvent(new MatchEvent()
            {
                Type = MatchEventType.CardAttackedSide,
                PrimaryCardId = _attackerCardID,
                SecondarySideIndex = _defenderSideIndex
            });
        }
    }
}