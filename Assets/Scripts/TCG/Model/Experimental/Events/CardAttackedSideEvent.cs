namespace TCG.Model.Experimental.Events
{
    public static class CardAttackedSideEvent
    {
        public static int AttackerCardId(MatchEvent matchEvent) => matchEvent.PrimaryCardId;
        public static int AttackedSideIndex(MatchEvent matchEvent) => matchEvent.PrimarySideIndex;
        
        
        public static MatchEvent Create(int attackerCardId, int attackedSideIndex)
        {
            return new MatchEvent()
            {
                Type = MatchEventType.CardAttackedSide,
                PrimaryCardId = attackerCardId,
                PrimarySideIndex = attackedSideIndex,
            };
        }
    }
}