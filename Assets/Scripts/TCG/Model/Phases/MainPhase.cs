using TCG.Model.Cards;
using TCG.Model.Core;
using TCG.Model.Actions;

using UnityEngine;
using System.Linq;

namespace TCG.Model.Phases
{
    public class MainPhase : TurnPhase
    {
        public static readonly MainPhase Instance = new MainPhase();

        private MainPhase() : base()
        { }
    
        public override void Enter(Match match)
        {
            Debug.Log("Main1 Phase Enter");
        }
    
        public override void Execute(Match match)
        {
            if (string.IsNullOrEmpty(match.PendingInput))
            {
                match.OutputQueue.Enqueue("Expected: Play_InstanceID_LaneIndex_Front/Back or end");
                return;
            }

            // store it locally and clear no double processing
            string inputToProcess = match.PendingInput;
            match.ClearInput();

            if (inputToProcess.ToLower() == "end")
            {
                match.AdvancePhase();
            }
            else if (inputToProcess.ToLower().StartsWith("play"))
            {
                ParseSummon(inputToProcess, match);
            }
            else
            {
                match.OutputQueue.Enqueue("Expected: Play_InstanceID_LaneIndex_Front/Back or end");
            }
        }

        public override void Exit(Match match)
        {
            Debug.Log("Main Phase Exit");
        }

        public override TurnPhase GetNextPhase()
        {
            return BattlePhase.Instance;
        }

        private void ParseSummon(string input, Match match)
        {
            string[] parts = input.Split('_');
            if (parts.Length == 4 && parts[0].ToLower() == "play")
            {
                if (!int.TryParse(parts[1], out int instanceId))
                {
                    match.OutputQueue.Enqueue("InstanceID is invalid");
                    return;
                }
                if (!int.TryParse(parts[2], out int laneIndex))
                {
                    match.OutputQueue.Enqueue("laneIndex is invalid");
                    return;
                }
                if (laneIndex > 2 || laneIndex < 0)
                {
                    match.OutputQueue.Enqueue("laneIndex is invalid must be between 0 and 2");
                    return;
                }
                bool playToFront = parts[3].ToLower() == "front";
                PlayCardFromHand(match, instanceId, laneIndex, playToFront);
            }
        }

        public void PlayCardFromHand(Match match, int instanceId, int laneIndex, bool playToFront)
        {
            Side activeSide = match.Sides[match.ActiveSideIndex];

            // 1. Validate the card exists in hand
            RuntimeCard cardToPlay = activeSide.Hand.Find(c => c.InstanceId == instanceId);
            if (cardToPlay == null)
            {
                // Validation failed! 
                // Send the error to the UI/Console queue
                match.OutputQueue.Enqueue($"Instance ID {instanceId} is not found in your hand. there are {activeSide.Hand.Count}" +
                                          $"\n cards in your hand. {string.Join(", ", activeSide.Hand.Select(p => p.InstanceId))}" );
                return;
            }
            // 2. Validation passed! Pass the execution down to the mechanic.
            match.ActionStack.Push( new NormalSummonAction(match.ActiveSideIndex, instanceId, laneIndex, playToFront));
        }
    }
}



