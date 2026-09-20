using TCG.Model.Enums;
using TCG.Model.Events;

namespace TCG.View.Events
{
    public class PushAbyssEvent : ViewEvent
    {
        public readonly VerticalPushDirection Direction;
        public readonly int CardSideIndex;
        public readonly int CardId;

        public PushAbyssEvent(int cardSideIndex, int cardId, VerticalPushDirection direction)
        {
            Direction = direction;
            CardSideIndex = cardSideIndex;
            CardId = cardId;
        }
    }
}