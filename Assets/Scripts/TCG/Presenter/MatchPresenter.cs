using System;
using System.Collections.Generic;
using TCG.Model.Cards;
using TCG.Model.Core;
using TCG.Model.Events;
using TCG.View;
using TCG.View.Events;

namespace TCG.Presenter
{
    public class MatchPresenter
    {
        private readonly Match _match;
        private readonly MatchView _view;
        private readonly HashSet<int> _introducedCards = new HashSet<int>();
        private readonly Dictionary<int,CardSnapshot> _cardSnapshots = new Dictionary<int,CardSnapshot>();

        public MatchPresenter(Match match, MatchView view)
        {
            _match = match;
            _view = view;
            _view.OnInputSubmitted += HandlePlayerInput;
        }

        public void StartMatch()
        {
            _match.InitializeMatch();
            ContinueMatch();
        }

        private void ContinueMatch()
        {
            InputRequest inputRequest = null;
            List<EventSnapshot> eventSnapshots = new List<EventSnapshot>();
            do // do-while (inputRequest is null)
            {
                inputRequest = _match.Resolve();
                while (_match.VisibleEventQueue.Count > 0)
                {
                    MatchEvent e = _match.VisibleEventQueue.Dequeue();
                    eventSnapshots.Add(new EventSnapshot()
                    {
                        MatchEvent = e,
                        CardSnapshot = e.IntroducedCardId != 0? new CardSnapshot(e.IntroducedCardId,_match) : null
                    });
                }
            } while (inputRequest is null);
            // this part bellow runs when inputRequest is no longer null
            List<List<EventSnapshot>> groups = GroupEventSnapshots(eventSnapshots);
            Queue<ViewEvent> viewEvents = new Queue<ViewEvent>();
            foreach (var group in groups)
            {
                ViewEvent ve = CreateViewEvent(group);
                _view.EnqueueNewEvent(ve);
            }
            _view.ShowOutput(inputRequest.DisplayMessage); 
            _view.ShowMoves(inputRequest.LegalMoves);
            _view.ShowState(_match.GetStringState());
            _view.DisplayGameDisplay(new BoardSnapshot(_match));
        }

        private ViewEvent CreateViewEvent(List<EventSnapshot> group)
        {
            MatchEvent primaryEvent = group[0].MatchEvent;

            return primaryEvent switch
            {
                DrawnEvent => new DrawEvent(group),
                NormalSummonedEvent => new NormalSummonEvent(group),
                _ => throw new NotImplementedException($"No ViewEvent mapping for {primaryEvent.GetType().Name}")
            };
        }
            
        private List<List<EventSnapshot>> GroupEventSnapshots(List<EventSnapshot> eventSnapshots)
        {
            List<List<EventSnapshot>> groups = new List<List<EventSnapshot>>();

            int currentGroupId = -1;

            foreach (EventSnapshot snapshot in eventSnapshots)
            {
                if (snapshot.MatchEvent.GroupId != currentGroupId)
                {
                    currentGroupId = snapshot.MatchEvent.GroupId;
                    groups.Add(new List<EventSnapshot>());
                }

                groups[^1].Add(snapshot);
            }

            return groups;
        }
        
        private void HandlePlayerInput(PlayerInput playerInput)
        {
            _match.ReceiveInput(playerInput);
            ContinueMatch();
        }
    }
}