using TCG.Model.Cards;
using TCG.Model.Core;
using TCG.Model.Enums;
using TCG.Model.Events;

namespace TCG.Model.Actions
{
    public class AttackCardAction : MatchAction
    {
        private readonly int _attackerCardID;
        private readonly int _defenderCardID;

        public AttackCardAction(int attackerCardID, int defenderCardID, int groupId)
        {
            _attackerCardID = attackerCardID;
            _defenderCardID = defenderCardID;
            GroupId = groupId;
        }

        public override void Execute(Match match)
        {
            base.UpdateGroupID(match);
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
            match.PushAction( new DamageCardAction( _attackerCardID , _defenderCardID, amount, DamageType.AttackDamage, GroupId) );
            match.EnqueueEvent(new AttackedCardEvent(_attackerCardID, _defenderCardID, GroupId));
        }
    }
}