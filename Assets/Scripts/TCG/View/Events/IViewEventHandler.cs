namespace TCG.View.Events
{
    public interface IViewEventHandler
    {
        void EnqueueEvent(ViewEvent viewEvent);
    }
}