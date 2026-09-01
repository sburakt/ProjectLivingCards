namespace TCG.Model.Experimental.Events
{
    public static class CardEnteredFieldEvent
    {
        public static int CardId(MatchEvent matchEvent) => matchEvent.PrimaryCardId;
        public static int SideIndex(MatchEvent matchEvent) => matchEvent.PrimarySideIndex;

        public static MatchEvent Create(int cardId, int sideIndex)
        {
            return new MatchEvent()
            {
                Type = MatchEventType.CardEnteredField,
                PrimaryCardId = cardId,
                PrimarySideIndex = sideIndex,
            };
        }

    }
}