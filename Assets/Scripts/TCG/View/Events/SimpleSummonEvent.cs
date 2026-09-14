using TCG.Model.Core;

namespace TCG.View.Events
{
    public class SimpleSummonEvent : ViewEvent
    {
        public readonly int SummoningSideIndex;
        public readonly int SummonedCardId;
        public readonly Position SummonedPosition;

        public SimpleSummonEvent(int summoningSideIndex, int summonedCardId, Position summonedPosition)
        {
            SummoningSideIndex = summoningSideIndex;
            SummonedCardId = summonedCardId;
            SummonedPosition = summonedPosition;
        }
    }
}