using TCG.Model.Cards;
using TCG.Model.Core;

namespace TCG.Model.Actions
{
    public class StandardLaneBattleAction : MatchAction
    {
        private readonly int _laneIndex;
        private readonly int _attackerSideIndex;

        public StandardLaneBattleAction(int laneIndex, int attackerSideIndex)
        {
            _laneIndex = laneIndex;
            _attackerSideIndex = attackerSideIndex;
        }

        public override void Execute(Match match)
        {
            Side attackerSide = match.Sides[_attackerSideIndex];
            Side defenderSide = match.Sides[_attackerSideIndex ^ 1];
            Lane attackerLane = attackerSide.Field.Lanes[_laneIndex];
            Lane defenderLane = defenderSide.Field.Lanes[_laneIndex];
            RuntimeCard attacker = attackerLane.FrontCard;
            RuntimeCard defender = null;
            if (defenderLane.FrontCard != null)
                defender = defenderLane.FrontCard;
            else if (defenderLane.BackCard != null)
                defender = defenderLane.BackCard;

            // 3. THE PRE-CHECK & FIZZLE
            // If the attacker was destroyed or removed before this action resolved, fizzle!
            if (attacker == null) // || attacker.HasModifier(Modifier.CannotAttack)) 
            {
                return;
            }

            // 4. THE MATH
            if (defender != null)
            {
                match.ActionStack.Push(new CardAttackCardAction(_attackerSideIndex, attacker.InstanceId,
                    defender.InstanceId));
                // action stack push attack card
            }
            else
            {
                match.ActionStack.Push(new CardAttackSideAction(_attackerSideIndex, attacker.InstanceId));
                // Direct attack on the player
            }
        }
    }
}