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
            
            Cell attackerFrontCell = attackerLane.FrontCell;
            
            // fizzle check
            if (attackerFrontCell.IsEmpty)
                return;

            RuntimeCard attacker = attackerFrontCell.Card;
            
            if (defenderLane.IsEmpty)
            {
                match.PushAction(new CardAttackSideAction(_attackerSideIndex, attacker.InstanceId));
            }
            else
            {
                
                RuntimeCard defenderCard = defenderLane.FrontCell.IsFull? defenderLane.FrontCell.Card : defenderLane.BackCell.Card;
                match.PushAction(
                    new CardAttackCardAction(_attackerSideIndex, attacker.InstanceId, defenderCard.InstanceId)
                    );
                
            }  
        }
    }
}