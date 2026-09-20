using System.Collections.Generic;
using TCG.Model.Events;
using TCG.Presenter;
using TCG.Model.Enums;

namespace TCG.View.Events
{
    public class StandardLaneBattleEvent : ViewEvent
    {
        public int AttackerSideIndex { get; private set; }
        public int AttackedSideIndex { get; private set; }
        public int LaneIndex { get; private set; }
        
        public BattleType BattleType { get; private set; }
        
        public AttackSideEvent AttackSideEvent  { get; private set; }
        
        public DamageSideEvent DamageSideEvent { get; private set; }
        
        public AttackCardEvent AttackCardEvent { get; private set; }
        
        public DamageCardEvent DamageCardEvent { get; private set; }
        public DestroyEvent DestroyEvent { get; private set; }
        

        public StandardLaneBattleEvent(List<EventSnapshot> group)
        {
            if (group[0].MatchEvent is StandardLaneBattledEvent standardLaneBattleEvent)
            {
                AttackerSideIndex = standardLaneBattleEvent.AttackerSideIndex;
                AttackedSideIndex = AttackerSideIndex ^ 1;
                LaneIndex = standardLaneBattleEvent.LaneIndex;
                BattleType = standardLaneBattleEvent.BattleType;
            }
            foreach (EventSnapshot eventSs in group)
            {
                if (eventSs.MatchEvent is AttackedSideEvent attackedSideEvent)
                {
                    int attackingCardId = attackedSideEvent.AttackerId;
                    AttackSideEvent = new AttackSideEvent(AttackerSideIndex, attackingCardId, AttackedSideIndex);
                }
                if (eventSs.MatchEvent is DamagedSideEvent damagedSideEvent)
                {
                    int damageAmount = damagedSideEvent.DamageAmount;
                    DamageSideEvent = new DamageSideEvent(AttackedSideIndex, damageAmount);
                }
                if (eventSs.MatchEvent is AttackedCardEvent attackedCardEvent)
                {
                    int attackingCardId = attackedCardEvent.AttackerId;   
                    int attackedCardId  = attackedCardEvent.DefenderId;
                    AttackCardEvent = new AttackCardEvent(AttackerSideIndex, attackingCardId, AttackedSideIndex, attackedCardId);
                }
                if (eventSs.MatchEvent is DamagedCardEvent damagedCardEvent)
                {
                    int damageAmount = damagedCardEvent.DamageAmount;
                    DamageCardEvent = new DamageCardEvent(AttackCardEvent.DefenderCardId, damageAmount);
                }

                if (eventSs.MatchEvent is DestroyedEvent destroyedEvent)
                {
                    DestroyEvent = new DestroyEvent(destroyedEvent);
                }
            }
            
        }
    }
}