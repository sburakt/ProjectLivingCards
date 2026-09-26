using System;
using System.Collections.Generic;
using TCG.Model.Cards;
using TCG.Model.Effects;
using TCG.Model.Events;
using TCG.Model.Phases;
using TCG.Model.Actions;

using System.Linq;
using UnityEngine;

namespace TCG.Model.Core
{
    public class Match
    {
        // match vars
        private int _nextInstanceID = 1;
        private int _nextGroupID = 1;
        public const int UNASSIGNED_GROUP_ID = 0;
        private bool _matchOver = false;
        private readonly Player[] _players = new Player[2];

        // side variables
        private readonly Side[] _sides = new Side[2];
        public IReadOnlyList<Side> Sides => _sides;
        public int ActiveSideIndex { get; private set; } = 0;
        public Side ActiveSide => _sides[ActiveSideIndex];
        public Side OpponentSide => _sides[ActiveSideIndex ^ 1];

        // abyss
        public readonly Abyss Abyss = new Abyss();
        // phase variables
        private TurnPhase _currentPhase;
        private int _turnCount = 1;
        public int CurrentPhaseStep;

        // gameplay variables
        private readonly List<Effect> _effects = new List<Effect>();
        public  IReadOnlyList<Effect> EffectList => _effects;
        private readonly Stack<MatchAction> _actionStack = new Stack<MatchAction>();
        private readonly Queue<MatchEvent> _eventQueue = new Queue<MatchEvent>();
        private readonly Queue<MatchEvent> _reactQueue = new Queue<MatchEvent>();
        public readonly Queue<MatchEvent> VisibleEventQueue = new Queue<MatchEvent>();

        public void PushAction(MatchAction action)
        {
            _actionStack.Push(action);
        }

        public void EnqueueEvent(MatchEvent mEvent)
        {
            _eventQueue.Enqueue(mEvent);
        }
        
        public void AddEffect(Effect effect) => _effects.Add(effect);

        private void RemoveTerminatedEffects()
        {
            _effects.RemoveAll(effect => effect.ShouldTerminate(this));
        }

        private string _pendingStringInputRequest;
        private InputRequest _pendingInputRequest;

        public void RequestInput(InputRequest inputRequest)
        {
            if (_pendingInputRequest != null)
            {
                Debug.Log($"Requesting input request: {inputRequest.DisplayMessage} but pending input request: {_pendingInputRequest.DisplayMessage} is not resolved" );
            }
            _pendingInputRequest = inputRequest;
        }

        // we get the move out of input right away
        // but might change it if input with no moves will be needed for very spesific effects in future
        //public PlayerInput PendingInput { get; private set; }
        
        private PlayerMove _pendingMove;

        public void ReceiveInput(PlayerInput input)
        {
            if (_pendingInputRequest is null)
                throw new InvalidOperationException("No input is requested.");

            if (input.OptionIndex < 0 || input.OptionIndex >= _pendingInputRequest.LegalMoves.Count)
                throw new ArgumentOutOfRangeException(nameof(input.OptionIndex));

            _pendingMove = _pendingInputRequest.LegalMoves[input.OptionIndex];
            _pendingInputRequest = null;
        }


        public PlayerMove ConsumeMove()
        {
            PlayerMove temp = _pendingMove;
            _pendingMove = null;
            return temp;
        }

        public Match(Player p1, Player p2)
        {
            _players[0] = p1;
            _players[1] = p2;
            _sides[0] = new Side(0);
            _sides[1] = new Side(1);
        }


        public void InitializeMatch()
        {
            for (int i = 0; i < 2; i++)
            {
                _sides[i].Deck = CreateRuntimeCards(_players[i].PersistentDeck, _sides[i]);
            }

            // more initializaion code here
            Debug.Log("Initialized Match");
            Debug.Log($"deck size is {_sides[0].Deck.Count}");
            ChangePhase(SetupPhase.Instance);
        }

        public InputRequest Resolve()
        {
            // safety check
            if (_pendingInputRequest is not null)
            {
                Debug.LogWarning("Resolve() called with pending input request — input not yet received.");
                return _pendingInputRequest;
            }
            Tick();
            return _pendingInputRequest;
        }

        private void Tick()
        {
            if (_actionStack.Count > 0)
            {
                _actionStack.Pop().Execute(this);
                ProcessEvents();
                return;
            }
            if (_reactQueue.Count > 0)
            {
                ReactEvents();
                RemoveTerminatedEffects();
                return;
            }
            _currentPhase.Execute(this);
        }

        private void ProcessEvents()
        {
            while (_eventQueue.Count > 0)
            {
                MatchEvent e = _eventQueue.Dequeue();
                VisibleEventQueue.Enqueue(e);
                _reactQueue.Enqueue(e);
            }
        }

        private void ReactEvents()
        {
            while (_reactQueue.Count > 0)
            {
                MatchEvent currentEvent = _reactQueue.Dequeue();
                foreach (Effect effect in _effects)
                {
                    if (effect is IReactiveEffect reactive)
                        reactive.React(this, currentEvent);
                }
            }
        }


        public void PassTurn()
        {
            Debug.Log("Passing turn");
            ActiveSideIndex = ActiveSideIndex ^ 1;
            ChangePhase(DrawPhase.Instance);
            _turnCount++;
        }

        public void AdvancePhase()
        {
            // Get the next phase from the current state
            TurnPhase nextPhase = _currentPhase.GetNextPhase();

            // Use ChangePhase to ensure Exit() and Enter() run!
            ChangePhase(nextPhase);
        }

        private void ChangePhase(TurnPhase newPhase)
        {
            _currentPhase?.Exit(this);
            _currentPhase = newPhase;
            _currentPhase?.Enter(this);
        }

        public Cell GetCell(Position position)
        {
            Cell cell = position.IsFront ?
                _sides[position.SideIndex].Field.Lanes[position.LaneIndex].FrontCell:
                _sides[position.SideIndex].Field.Lanes[position.LaneIndex].BackCell;
            return cell;
        }
                
        // for test only
        public string GetStringState()
        {
            string state = "=== CURRENT GAME STATE ===\n";

            for (int i = 0; i < 2; i++)
            {
                Side side = _sides[i];

                // Grab all the Card IDs currently in the hand
                string handCards = string.Join(", ", side.Hand.Select(c => c.StaticCard.CardId));
                if (string.IsNullOrEmpty(handCards)) handCards = "Empty";

                state += $"[Player {i + 1}] {(i == ActiveSideIndex ? "(ACTIVE)" : "")}\n";
                state += $"Life Points: {side.LifePoints} \n";
                state += $"Deck: {side.Deck.Count} cards\n";
                state += $"Hand ({side.Hand.Count}): {handCards}\n";
                state += $"Graveyard: {side.Graveyard.Count}\n";
                state += $"Field:\n{side.Field.Lanes[0].FrontCard?.StaticCard.CardId ?? "empty"}" +
                         $" {side.Field.Lanes[1].FrontCard?.StaticCard.CardId ?? "empty"}" +
                         $" {side.Field.Lanes[2].FrontCard?.StaticCard.CardId ?? "empty"}\n" +
                         $"{side.Field.Lanes[0].BackCard?.StaticCard.CardId ?? "empty"}" +
                         $" {side.Field.Lanes[1].BackCard?.StaticCard.CardId ?? "empty"}" +
                         $" {side.Field.Lanes[2].BackCard?.StaticCard.CardId ?? "empty"}\n\n";
            }

            state += "==========================";

            return state;
        }

        public BoardSnapshot GetSnapshot()
        {
            return new BoardSnapshot(this);
        }

        // might be improved or moved out of match class

        public int ResolveGroupId(int number)
        {
            if (number != UNASSIGNED_GROUP_ID)
                return number;
            return _nextGroupID++;
        }

        public RuntimeCard FindRuntimeCardById(int targetId, int expectedSideHint = 0)
        {
            RuntimeCard fastResult = Sides[expectedSideHint].FindRuntimeCardById(targetId);
            if (fastResult != null) return fastResult;

            int otherSide = expectedSideHint ^ 1;
            RuntimeCard slowResult = Sides[otherSide].FindRuntimeCardById(targetId);
            if (slowResult != null) return slowResult;
            Debug.LogWarning($"no card with the id {targetId}");
            return null;
        }
        private List<RuntimeCard> CreateRuntimeCards(List<PersistentCard> persistentCards, Side side)
        {
            List<RuntimeCard> runtimeCards = new();
            foreach (var persistentCard in persistentCards)
            {
                RuntimeCard runtimeCard = new RuntimeCard(_nextInstanceID++, persistentCard, side, side);
                // add any debuff to runtime card here
                runtimeCards.Add(runtimeCard);
            }

            return runtimeCards;
        }

    }
}