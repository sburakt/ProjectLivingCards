namespace TCG.Model.Events
{
    public static class CardDestroyCardEvent
    {
        public static int DestroyerCardId(MatchEvent matchEvent) => matchEvent.PrimaryCardId;
        public static int DestroyedCardId(MatchEvent matchEvent) => matchEvent.SecondaryCardId;
        public static DestroyCause Cause(MatchEvent matchEvent) => (DestroyCause)matchEvent.EnumAsInt;
        
        
        public static MatchEvent Create(int destroyerCardId, int destroyedCardId, DestroyCause cause)
        {
            return new MatchEvent()
            {
                Type = MatchEventType.CardDestroyedCard,
                PrimaryCardId = destroyerCardId,
                SecondaryCardId = destroyedCardId,
                EnumAsInt = (int)cause,
            };
        }

        public enum DestroyCause
        {
            BattleDamage,
            EffectDamage,
            DestroyEffect,
            Push
        }
    }
}