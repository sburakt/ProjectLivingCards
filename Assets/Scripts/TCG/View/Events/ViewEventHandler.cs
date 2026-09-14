using System.Collections.Generic;
using TCG.View.Events;
using TCG.View.Animations;
using UnityEngine;

namespace TCG.View.Events
{
    public class ViewEventHandler : MonoBehaviour, IViewEventHandler
    {
        public DrawAnimation drawAnimation;
        public NormalSummonAnimation normalSummonAnimation;
        private bool _processing = false;
        private readonly Queue<ViewEvent> _viewEvents = new Queue<ViewEvent>();

        public void EnqueueEvent(ViewEvent viewEvent)
        {
            _viewEvents.Enqueue(viewEvent);
            if (!_processing)
                ProcessNext();
        }
        private void ProcessNext()
        {
            _processing = true;
            ViewEvent ve = _viewEvents.Dequeue();
            switch (ve)
            {
                case DrawEvent drawEvent:
                    drawAnimation.Play(drawEvent,OnAnimationComplete);
                    break;
                case NormalSummonEvent summonEvent:
                    normalSummonAnimation.Play(summonEvent, OnAnimationComplete);
                    break;
            }
        }

        private void OnAnimationComplete()
        {            
            if (_viewEvents.Count > 0)
                ProcessNext();
            else
                _processing = false;
        }
    }
}