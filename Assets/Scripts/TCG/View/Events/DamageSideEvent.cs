using System;

namespace TCG.View.Events
{
    public class DamageSideEvent: ViewEvent
    {
        public int DamageSideIndex;
        public int Amount;

        public DamageSideEvent(int damageSideIndex, int amount)
        {
            DamageSideIndex = damageSideIndex;
            Amount = amount;
        }
    }
}