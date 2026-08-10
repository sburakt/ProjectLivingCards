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
        private bool _matchOver = false;
        private readonly Player[] _players = new Player[2];

        // side variables
        private readonly Side[] _sides = new Side[2];
        public IReadOnlyList<Side> Sides => _sides;
        public int ActiveSideIndex { get; private set; } = 0;
        public Side ActiveSide => _sides[ActiveSideIndex];
        public Side OpponentSide => _sides[ActiveSideIndex ^ 1];

        // phase variables
        private TurnPhase _currentPhase;
        private int _turnCount = 1;
        public int CurrentPhaseStep;

        // gameplay variables
        private readonly List<Effect> _effects = new List<Effect>();
        public  IReadOnlyList<Effect> EffectList => _effects;
        private readonly Stack<MatchAction> _actionStack = new Stack<MatchAction>();
        private readonly Queue<MatchEvent> _eventQueue = new Queue<MatchEvent>();
        public readonly Queue<MatchEvent> EventLog = new Queue<MatchEvent>();

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

        // test variables (temp)
        //public Queue<string> OutputQueue { get; } = new Queue<string>();
        
        private string _pendingInputRequest;
        
        public void RequestInput(string output) 
        {
            _pendingInputRequest = output;
        }

        public string ConsumeInputRequest()
        {
            string temp = _pendingInputRequest;
            _pendingInputRequest = null;
            return temp;
        }

        public string PendingInput { get; private set; }

        public void ReceiveInput(string input) 
        {
            PendingInput = input;
        }

        public void ClearInput()
        {
            PendingInput = null;
        }

        public enum TurnState
        {
            Draw,
            Main1,
            Battle,
            Main2,
            End
        }

        public Match(Player p1, Player p2)
        {
            _players[0] = p1;
            _players[1] = p2;
            _sides[0] = new Side();
            _sides[1] = new Side();
            InitializeMatch();
        }


        private void InitializeMatch()
        {
            for (int i = 0; i < 2; i++)
            {
                _sides[i].Deck = CreateRuntimeCards(_players[i].PersistentDeck, _sides[i]);
            }

            // more initializaion code here
            Debug.Log("Initialized Match");
            Debug.Log($"deck size is {_sides[0].Deck.Count}");
            Debug.Log(_sides[0].Deck[0].BaseDefense);
            ChangePhase(DrawPhase.Instance);
        }

        public void Resolve()
        {
            while (!_matchOver)
            {
                // if we asked for an input we must wait for it priority 1
                if (_pendingInputRequest is not null) // for now we use output queue i know it shouldnt excide 1 but that what we already have in hand and we use this for now
                {
                    break;
                }
                Tick();
            }
        }

        private void Tick()
        {
                // before phase execution we must see if stack empty
            if (_actionStack.Count > 0) // no internak while loop 
            {
                _actionStack.Pop().Execute(this);
            }
            else // call phase execute 
            {
                _currentPhase.Execute(this);
            }
            ProcessEvents();
            RemoveTerminatedEffects();
        }

        private void ProcessEvents()
        {
            while (_eventQueue.Count > 0)
            {
                MatchEvent currentEvent = _eventQueue.Dequeue();
                foreach (Effect effect in _effects)
                {
                    if (effect is IReactiveEffect reactive)
                        reactive.React(this, currentEvent);
                }
                EventLog.Enqueue(currentEvent);
            }
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

        public RuntimeCard FindRuntimeCardById(int targetId, int expectedSideHint = 0)
        {
            RuntimeCard fastResult = Sides[expectedSideHint].FindRuntimeCardById(targetId);
            if (fastResult != null) return fastResult;

            int otherSide = expectedSideHint ^ 1;
            RuntimeCard slowResult = Sides[otherSide].FindRuntimeCardById(targetId);
            if (slowResult != null) return slowResult;
            // If it's truly gone, return null to fizzle the action safely
            return null;
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

        // might be improved or moved out
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