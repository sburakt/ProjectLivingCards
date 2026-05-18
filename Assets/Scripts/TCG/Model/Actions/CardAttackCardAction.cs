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
            int remainingDamage = attackerCard.CurrentAttack;

            //Fizzle checks

            // check if any card is null

            // check if attacker card is on the attacker field

            // check if defender card is on the defender field

            // check for any flag that cancels action ig cannott attack or cannot be attacked

            if (defenderCard.CurrentDefense > 0)
            {
                // How much shield is left after taking the hit? (Can be negative)
                int shieldDifference = defenderCard.CurrentDefense - remainingDamage;

                if (shieldDifference >= 0)
                {
                    // Shield absorbed the whole hit! 
                    defenderCard.ChangeCurrentDefense(shieldDifference);
                    remainingDamage = 0;
                }
                else
                {
                    // Shield broke! The leftover damage (made positive) spills over.
                    defenderCard.ChangeCurrentDefense(0);
                    remainingDamage = Math.Abs(shieldDifference);
                }
            }

            // 3. Apply remaining damage to Health
            if (remainingDamage > 0)
            {
                defenderCard.ChangeCurrentHealth(defenderCard.CurrentHealth - remainingDamage);
                Debug.Log(
                    $"{attackerCard.StaticCard.CardId} dealt {remainingDamage} damage to {defenderCard.StaticCard.CardId}. HP left: {defenderCard.CurrentHealth}");
            }

            // 5. Check for Death
            if (defenderCard.CurrentHealth <= 0)
            {
                match.PushAction(new DestroyCardAction(_defenderCardID,_attackerSideIndex ^ 1));
                //destruction by battle should be spesified but dont know where can be refactored later
                Debug.Log($"{defenderCard.StaticCard.CardId} was destroyed!");
            }
        }
    }
}