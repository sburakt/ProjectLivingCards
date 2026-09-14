using System;
using System.Collections.Generic;
using TCG.Model.Enums;
using TCG.Model.Events;
using TCG.Presenter;

namespace TCG.View.Events
{
    public class DrawEvent : ViewEvent
    {
        public readonly int DrawingSideIndex;
        public List<int> CardsToDraw = new List<int>();
        public List<CardSnapshot> CardSnapshots = new List<CardSnapshot>();
        
        public DrawEvent(List<EventSnapshot> group)
        {
            if (group[0].MatchEvent is DrawnEvent primaryEvent)
            {
                DrawingSideIndex = primaryEvent.DrawingSideIndex;
            }
            foreach (EventSnapshot eventSnapshot in group)
            {
                if (eventSnapshot.MatchEvent is DrawnEvent drawnEvent)
                {
                    CardsToDraw.Add(drawnEvent.DrawnCardId);
                    CardSnapshots.Add(eventSnapshot.CardSnapshot);
                }
                else
                {
                    throw new Exception("Group with primary DrawnEvent contains non-DrawnEvent");
                }
            }
        }
    }
}