using TCG.Model.Cards;
using TCG.Model.Core;
using TCG.Model.Events;
using UnityEngine;

namespace TCG.Model.Actions
{
    public class CardAttackSideAction : MatchAction
    {
        private readonly int _attackerSideIndex;
        private readonly int _attackerCardID;

        public CardAttackSideAction(int attackerSideIndex, int attackerCardID)
        {
            _attackerSideIndex = attackerSideIndex;
            _attackerCardID = attackerCardID;

        }

        public override void Execute(Match match)
        {
            RuntimeCard attackerCard = match.FindRuntimeCardById(_attackerCardID, _attackerSideIndex);
            Side defenderSide = match.Sides[_attackerSideIndex ^ 1];
            int damage = attackerCard.CalculateStat(match, RuntimeCard.StatType.Attack);

            //Fizzle checks

            // check if attacker card is null

            // check if attacker card is on the attacker field

            // check for any flag that cancels action ig cannott attack or no direct attack


            // 1. Safety check (just in case attack was reduced to 0 by a debuff)
            if (damage > 0)
                match.EnqueueEvent(new MatchEvent()
                {
                    Type = MatchEventType.PlayerDamaged,
                    SourceId = _attackerSideIndex ^ 1,
                    Value = damage
                });

            // 2. Apply damage directly to the player's health. 
            // (Adjust 'Health' depending on where your health integer is stored!)
            defenderSide.LifePoints -= damage;

            Debug.Log(
                $"{attackerCard.StaticCard.CardId} dealt {damage} direct damage! Defender HP left: {defenderSide.LifePoints}");

            // 3. Check for Game Over condition
            if (defenderSide.LifePoints <= 0)
            {
                // Prevent health from showing as a negative number in the UI
                defenderSide.LifePoints = 0;

                Debug.Log("Player defeated! Game Over!");

                // You would trigger your match-ending logic here in the future
                // match.TriggerGameOver(winner: attackerSide);
            }
            match.EnqueueEvent(new MatchEvent()
            {
                Type = MatchEventType.CardAttackSide,
                SourceId = _attackerCardID,
                Value = damage
            });
        }
    }
}