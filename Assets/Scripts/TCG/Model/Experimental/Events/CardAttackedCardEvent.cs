namespace TCG.Model.Experimental.Events
{
    public static class CardAttackedCardEvent
    {
        public static int AttackerCardId(MatchEvent e) => e.PrimaryCardId;
        public static int DefenderCardId(MatchEvent e) => e.SecondaryCardId;

        public static MatchEvent Create(int attackerCardId, int defenderCardId)
        {
            return new MatchEvent()
            {
                Type = MatchEventType.CardAttackedCard,
                PrimaryCardId = attackerCardId,
                SecondaryCardId = defenderCardId
            };
        }
    }
}