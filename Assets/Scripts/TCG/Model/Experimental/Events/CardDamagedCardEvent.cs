using TCG.Model.Enums;

namespace TCG.Model.Experimental.Events
{
    public static class CardDamagedCardEvent
    {
        public static int HittingCardId(MatchEvent e) => e.PrimaryCardId;
        public static int DamagedCardId(MatchEvent e) => e.SecondaryCardId;
        public static DamageType DamageType(MatchEvent e) => (DamageType)e.EnumAsInt;

        public static MatchEvent Create(int hittingCardId, int damagedCardId, DamageType damageType)
        {
            return new MatchEvent()
            {
                Type = MatchEventType.CardDamagedCard,
                PrimaryCardId = hittingCardId,
                SecondaryCardId = damagedCardId,
                EnumAsInt = (int)damageType,
            };
        }

    }
}