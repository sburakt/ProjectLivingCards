using TCG.Model.Core;
using TCG.Model.Events;

namespace TCG.View.Events
{
    public class PushMoveEvent : ViewEvent
    {
        public int MovedCardId;
        public Position NewPosition;
        //public int VisuallyPushingCardId;

        public PushMoveEvent(MovedEvent moveEvent) //, int visuallyPushingCardId)
        {
            MovedCardId = moveEvent.MovedCardId;
            NewPosition = moveEvent.NewPosition;
            //VisuallyPushingCardId = visuallyPushingCardId;
        }
    }
}