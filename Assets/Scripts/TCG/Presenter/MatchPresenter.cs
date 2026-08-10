using TCG.Model.Core;
using TCG.Model.Events;
using UnityEngine;

namespace TCG.Presenter
{
    public class MatchPresenter
    {
        private readonly Match _match;
        private readonly GameInitializer _gameInitializer;
        //private readonly 

        public MatchPresenter(Match match, GameInitializer gameInitializer)
        {
            _match = match;
            _gameInitializer = gameInitializer;
            _gameInitializer.OnMoveSubmitted += HandleUserInput;
        }

        public void StartMatch()
        {
            _match.Resolve();
            _gameInitializer.ShowState(_match.GetStringState());
            _gameInitializer.ShowOutput(_match.ConsumeInputRequest());
            AddAllLogsFromQueue();
            
        }

        private void AddAllLogsFromQueue()
        {
            while (_match.EventLog.TryDequeue(out var matchEvent))
            {
                _gameInitializer.AddLog(matchEvent.ToString());

            }
        }

        private void HandleUserInput(string inputString)
        {
            _match.ReceiveInput(inputString);
            _match.Resolve();
            _gameInitializer.ShowState(_match.GetStringState());
            _gameInitializer.ShowOutput(_match.ConsumeInputRequest());
            AddAllLogsFromQueue();
        }
        
    }
}