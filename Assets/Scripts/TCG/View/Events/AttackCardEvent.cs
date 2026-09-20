namespace TCG.View.Events
{
    public class AttackCardEvent : ViewEvent
    {
        public int AttackerSidedIndex { get; set; }   
        public int AttackerCardId { get; set; }
        public int DefenderSideIndex { get; set; }
        public int DefenderCardId { get; set; }

        public AttackCardEvent(int attackerSideIndex, int attackerCardId, int defenderSideIndex, int defenderCardId)
        {
            AttackerSidedIndex = attackerSideIndex;
            AttackerCardId = attackerCardId;
            DefenderSideIndex = defenderSideIndex;
            DefenderCardId = defenderCardId;
        }
    }
}