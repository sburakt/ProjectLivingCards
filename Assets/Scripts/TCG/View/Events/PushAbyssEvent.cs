using TCG.Model.Events;

namespace TCG.View.Events
{
    public class PushAbyssEvent : ViewEvent
    {
        public readonly int CardSideIndex;
        public readonly int CardId;

        public PushAbyssEvent(int cardSideIndex, int cardId)
        {
            CardSideIndex = cardSideIndex;
            CardId = cardId;
        }
    }
}