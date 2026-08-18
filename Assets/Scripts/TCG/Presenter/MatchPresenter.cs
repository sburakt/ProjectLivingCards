using TCG.Model.Core;

namespace TCG.Presenter
{
    public class MatchPresenter
    {
        private readonly Match _match;
        private readonly GameInitializer _gameInitializer;

        public MatchPresenter(Match match, GameInitializer gameInitializer)
        {
            _match = match;
            _gameInitializer = gameInitializer;
            _gameInitializer.OnMoveSubmitted += HandleUserInput;
        }

        public void StartMatch()
        {
            _match.InitializeMatch();
            ContinueMatch();
        }

        private void ContinueMatch()
        {
            _match.Resolve();
            _gameInitializer.ShowState(_match.GetStringState());
            _gameInitializer.ShowOutput(_match.ConsumeInputRequest());
            _gameInitializer.ShowMoves(_match.GetLegalMoves());
            AddAllLogsFromQueue();
            
        }

        private void HandleUserInput(PlayerMove move)
        {
            _match.ReceiveMove(move);
            ContinueMatch();
        }

        // test only
        private void AddAllLogsFromQueue() 
        {
            while (_match.EventLog.TryDequeue(out var matchEvent))
            {
                _gameInitializer.AddLog(matchEvent.ToString());

            }
        }
        
        // test  only
        private PlayerMove ParseMove(string input, Match match)
        {
            string[] parts = input.Split('_');
            if (input.ToLower() == "end")
            {
                return new EndTurnMove(match.ActiveSideIndex);
            }
            if (parts.Length == 4 && parts[0].ToLower() == "play")
            {
                if (!int.TryParse(parts[1], out int instanceId))
                {
                    match.RequestInput("InstanceID is invalid");
                    return null;
                }
                if (!int.TryParse(parts[2], out int laneIndex))
                {
                    match.RequestInput("laneIndex is invalid");
                    return null;
                }
                if (laneIndex > 2 || laneIndex < 0)
                {
                    match.RequestInput("laneIndex is invalid must be between 0 and 2");
                    return null;
                }
                bool playToFront = parts[3].ToLower() == "front";
                Position position = new Position()
                {
                    SideIndex = match.ActiveSideIndex,
                    LaneIndex = laneIndex,
                    IsFront = playToFront
                };
                return new PlayCardMove(_match.ActiveSideIndex, instanceId, position);
            }
            return null;
        }
        
    }
}