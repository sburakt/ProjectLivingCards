using System.Collections.Generic;
using TCG.Model.Cards;
using TCG.Model.Core;
using TCG.Model.Effects;
using TCG.Model.Events;
using TCG.View;
using UnityEngine;

namespace TCG.Presenter
{
    public class MatchPresenter
    {
        private readonly Match _match;
        //private readonly MatchView _view;
        private readonly MatchView _view;

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
            do // while (inputRequest is null) ftr: i hate do whiles but i had to use it here 
            {
                inputRequest = _match.Resolve();
                if (inputRequest is not null)
                {
                    // tick record
                }
            }
            while (inputRequest is null);
            // this part bellow runs when inputRequest no longer null
            _view.ShowOutput(inputRequest.DisplayMessage);
            _view.ShowMoves(inputRequest.LegalMoves);
            _view.DisplayGameDisplay(BuildGameDisplayData(inputRequest));
        }

        private void HandlePlayerInput(PlayerInput playerInput)
        {
            _match.ReceiveInput(playerInput);
            ContinueMatch();
        }
        
        // display Data Build

        public GameDisplayData BuildGameDisplayData(InputRequest inputRequest)
        {
            SideDisplayData[] sideDisplayData = new SideDisplayData[2];
            sideDisplayData[0] = BuildSideDisplayData(0);
            sideDisplayData[1] = BuildSideDisplayData(1);
            List<MatchEvent> eventLog = new List<MatchEvent>();
            while (_match.EventLog.TryDequeue(out var matchEvent))
            {
                eventLog.Add(matchEvent);
            }
            GameDisplayData gameDisplayData = new GameDisplayData()
            {
                Sides = sideDisplayData,
                ActiveSideIndex = _match.ActiveSideIndex,
                EventLog = eventLog,
                //LegalMoves = inputRequest.LegalMoves,
            };
            return gameDisplayData;
        }

        private SideDisplayData BuildSideDisplayData(int sideIndex)
        {
            Side side = _match.Sides[sideIndex];
            int lifePoints = side.LifePoints;
            List<CardDisplayData> hand = new List<CardDisplayData>();
            CardDisplayData[] field = new CardDisplayData[6];
            foreach (RuntimeCard card  in side.Hand)
            {
                hand.Add(BuildCardDisplayData(card));
            }
            int i = 0;
            foreach (RuntimeCard card in side.GetCardSlotsInField())
            {
                if (card is null)
                    field[i] = null;
                else
                    field[i] = BuildCardDisplayData(card);

                i++;
            }
            SideDisplayData sideDisplayData = new SideDisplayData()
            {
                LifePoints = side.LifePoints,
                Hand = hand,
                Field = field
            };
            return sideDisplayData;
        }

        private CardDisplayData BuildCardDisplayData(RuntimeCard runtimeCard)
        {
            CardDisplayData cardDisplayData = new CardDisplayData()
            {
                InstanceId = runtimeCard.InstanceId,
                CardName = runtimeCard.StaticCard.CardId,
                Attack = runtimeCard.CalculateStat(_match, RuntimeCard.StatType.Attack),
                BaseHealth = runtimeCard.BaseHealth,
                CurrentHealth = runtimeCard.CurrentHealth,
                Buffs = BuildBuffDisplayData(runtimeCard)
            };
            return cardDisplayData;
        }

        private List<BuffDisplayData> BuildBuffDisplayData(RuntimeCard runtimeCard)
        {
            List<BuffDisplayData> buffDisplayDatas = new List<BuffDisplayData>();
            int buffId;
            int? buffStack;
            foreach (Effect effect in _match.EffectList)
            {
                if (effect is IBuff buff)
                {
                    if (buff.TargetInstanceId == runtimeCard.InstanceId)
                    {
                        buffId = buff.BuffId;
                        buffStack = buff.Stack;
                        buffDisplayDatas.Add(new BuffDisplayData()
                        {
                            BuffId = buffId,
                            StackCount = buffStack
                        });
                    }
                }
            }
            return buffDisplayDatas;
        }

        // test only
        private void AddAllLogsFromQueue() 
        {
            while (_match.EventLog.TryDequeue(out var matchEvent))
            {
                _view.AddLog(matchEvent.ToString());
            }
        }
    }
}