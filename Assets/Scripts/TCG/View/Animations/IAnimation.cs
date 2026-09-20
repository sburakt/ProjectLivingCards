using System;
using TCG.View.Events;

namespace TCG.View.Animations
{
    public interface IAnimation<in TEvent> where TEvent : ViewEvent
    // note to self check the diff in and co var again some time
    {
        void Play(TEvent damageSideEvent, Action onComplete);
    }
}