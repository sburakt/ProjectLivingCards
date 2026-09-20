using System.Collections.Generic;
using TCG.Model.Enums;
using TCG.Model.Events;
using TCG.Presenter;
using UnityEngine;

namespace TCG.View.Events
{
    public class NormalSummonEvent : ViewEvent
    {
        private readonly int _summoningSideIndex;
        
        private VerticalPushDirection _verticalPushDirection;
        public SimpleSummonEvent SimpleSummonEvent { get; private set; }
        public PushAbyssEvent PushAbyssEvent { get; private set; }
        public PushMoveEvent PushMoveEvent { get; private set; }
        public DestroyEvent DestroyEvent { get; private set; }

        public NormalSummonEvent(List<EventSnapshot> group)
        {
            if (group[0].MatchEvent is NormalSummonedEvent normalSummonedEvent)
            {
                _summoningSideIndex = normalSummonedEvent.SummoningSideIndex;
                _verticalPushDirection = normalSummonedEvent.Direction;
            }
            foreach (EventSnapshot eventSs in group)
            {

                if (eventSs.MatchEvent is MovedEvent movedEvent)
                {
                    Debug.Log($"Moved Event ,{movedEvent.MovedCardId} , {movedEvent.NewPosition.ToString()}");
                    PushMoveEvent moveEvent = new PushMoveEvent(movedEvent);
                    PushMoveEvent = moveEvent;
                }

                else if (eventSs.MatchEvent is DestroyedEvent destroyedEvent)
                {
                    if (destroyedEvent.DestroyCause != DestroyCause.Push)
                        continue;
                    // always pushing back rn
                    PushAbyssEvent = new PushAbyssEvent(_summoningSideIndex, destroyedEvent.DestroyedId, _verticalPushDirection);
                    Debug.Log($"Destroyed Event ,{destroyedEvent.DestroyedId} , {destroyedEvent.DestroyerId}");
                    DestroyEvent  = new DestroyEvent(destroyedEvent);
                }

                else if (eventSs.MatchEvent is SimpleSummonedEvent summonedEvent)
                {
                    Debug.Log($"Simple Summon");
                    SimpleSummonEvent = new SimpleSummonEvent(_summoningSideIndex, summonedEvent.SummonedCardId, summonedEvent.SummonedPosition);
                }
                else if (eventSs.MatchEvent is PushedEvent pushedEvent)
                {
                    Debug.Log($"Pushed Event");
                }

                else if (eventSs.MatchEvent is EnteredFieldEvent)
                {
                    Debug.Log($"Entered Field");
                }

                else if (eventSs.MatchEvent is NormalSummonedEvent)
                {
                    Debug.Log($"NormalSummon");
                }
                else
                {
                    Debug.Log($"different event {eventSs.MatchEvent.GetType().Name}");
                }
            }
        }
    }
}