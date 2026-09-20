using TCG.Model.Events;

namespace TCG.View.Events
{
    public class AttackSideEvent : ViewEvent
    {
        public int AttackerSideIndex { get; set; }
        public int AttackerCardId { get; set; }
        public int DefenderSideIndex { get; set; }

        public AttackSideEvent(int attackerSideIndex,int attackerCardId, int defenderSideIndex)
        {
            AttackerSideIndex = attackerSideIndex;
            AttackerCardId = attackerCardId;
            DefenderSideIndex = defenderSideIndex;
        }
    }
}