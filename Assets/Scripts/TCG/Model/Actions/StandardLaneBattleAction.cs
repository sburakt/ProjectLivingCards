using TCG.Model.Cards;
using TCG.Model.Core;
using TCG.Model.Enums;
using TCG.Model.Events;

namespace TCG.Model.Actions
{
    public class StandardLaneBattleAction : MatchAction
    {
        private readonly int _laneIndex;
        private readonly int _attackerSideIndex;

        public StandardLaneBattleAction(int laneIndex, int attackerSideIndex, int groupId)
        {
            _laneIndex = laneIndex;
            _attackerSideIndex = attackerSideIndex;
            GroupId = groupId;
        }

        public override void Execute(Match match)
        {
            base.UpdateGroupID(match);
            Side attackerSide = match.Sides[_attackerSideIndex];
            Side defenderSide = match.Sides[_attackerSideIndex ^ 1];
            
            Lane attackerLane = attackerSide.Field.Lanes[_laneIndex];
            Lane defenderLane = defenderSide.Field.Lanes[_laneIndex];
            
            Cell attackerFrontCell = attackerLane.FrontCell;
            
            // fizzle check
            if (attackerFrontCell.IsEmpty)
            {
                match.EnqueueEvent(new StandardLaneBattledEvent( _laneIndex, _attackerSideIndex, BattleType.NoBattle, GroupId));
                return;
            }

            RuntimeCard attacker = attackerFrontCell.Card;
            
            if (defenderLane.IsEmpty)
            {
                match.PushAction(new AttackSideAction(attacker.InstanceId, _attackerSideIndex^1, GroupId));
                match.EnqueueEvent(new StandardLaneBattledEvent( _laneIndex, _attackerSideIndex, BattleType.AttackSide ,GroupId));
            }
            else
            {
                
                RuntimeCard defenderCard = defenderLane.FrontCell.IsFull? defenderLane.FrontCell.Card : defenderLane.BackCell.Card;
                match.PushAction(new AttackCardAction(attacker.InstanceId, defenderCard.InstanceId, GroupId));
                match.EnqueueEvent(new StandardLaneBattledEvent( _laneIndex, _attackerSideIndex, BattleType.AttackCard ,GroupId));
                
            }
            
        }
        
    }
}