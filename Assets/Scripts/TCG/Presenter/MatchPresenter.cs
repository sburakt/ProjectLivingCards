using System.Collections.Generic;
using TCG.Model.Cards;
using TCG.Model.Core;
using TCG.Model.Effects;
using TCG.View;

namespace TCG.Presenter
{
    public class MatchPresenter
    {
        private readonly Match _match;
        private readonly MatchView _view;
        private readonly GameInitializer _gameInitializer;

        public MatchPresenter(Match match, GameInitializer gameInitializer)
        {
            _match = match;
            _gameInitializer = gameInitializer;
            _gameInitializer.OnInputSubmitted += HandlePlayerInput;
        }

        public void StartMatch()
        {
            _match.InitializeMatch();
            ContinueMatch();
        }

        private void ContinueMatch()
        {
            InputRequest inputRequest = _match.Resolve();
            _gameInitializer.ShowOutput(inputRequest.DisplayMessage);
            _gameInitializer.ShowMoves(inputRequest.LegalMoves);
            _gameInitializer.ShowState(_match.GetStringState());
            // where we really build display data and log and pass to view in future
            AddAllLogsFromQueue(); // test only
        }

        private void HandlePlayerInput(PlayerInput playerInput)
        {
            _match.ReceiveInput(playerInput);
            ContinueMatch();
        }
        
        // display Data Build

        public GameDisplayData BuildGameDisplayData()
        {
            SideDisplayData[] sideDisplayData = new SideDisplayData[2];
            sideDisplayData[0] = BuildSideDisplayData(0);
            sideDisplayData[1] = BuildSideDisplayData(1);
            GameDisplayData gameDisplayData = new GameDisplayData()
            {
                Sides = sideDisplayData,
                ActiveSideIndex = _match.ActiveSideIndex,
                EventLog = new List<string>(), //empty for now test only
                //LegalMoves = _match.GetLegalMoves(),
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
                _gameInitializer.AddLog(matchEvent.ToString());
            }
        }
    }
}