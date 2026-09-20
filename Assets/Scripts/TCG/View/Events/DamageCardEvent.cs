namespace TCG.View.Events
{
    public class DamageCardEvent : ViewEvent
    {
        public int DamagedCardId;
        public int Amount;

        public DamageCardEvent(int damagedCardId, int amount)
        {
            DamagedCardId = damagedCardId;
            Amount = amount;
        }
    }
}