using TCG.Model.Events;

namespace TCG.View.Events
{
    public class DestroyEvent : ViewEvent
    {
        public int DestroyedCardId;
        public int DestroyerCardId;
        public Model.Enums.DestroyCause DestroyCause;

        public DestroyEvent(DestroyedEvent destroyedEvent)
        {
            DestroyedCardId = destroyedEvent.DestroyedId;
            DestroyerCardId = destroyedEvent.DestroyedId;
            DestroyCause = destroyedEvent.DestroyCause;
        }
    }
}