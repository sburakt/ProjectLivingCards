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
            PlayerMove move = match.ConsumeMove();
            if (move == null)
            {
                match.RequestInput($"Waiting for Main Phase to move from player{match.ActiveSideIndex}");
                return;
            }
            if (move is PlayCardMove playCardMove)
            {
                PlayCardFromHand(match, playCardMove.CardInstanceId, playCardMove.LaneIndex, playCardMove.IsFront);
                return;
            }
            if (move is EndTurnMove endTurnMove)
            {
                match.AdvancePhase();
                return;
            }
            //if (move is ActivateEffectMove activateEffectMove) {}
        }

        public override void Exit(Match match)
        {
            Debug.Log("Main Phase Exit");
        }

        public override TurnPhase GetNextPhase()
        {
            return BattlePhase.Instance;
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
                match.RequestInput($"Instance ID {instanceId} is not found in your hand. there are {activeSide.Hand.Count}" +
                                   $"\n cards in your hand. {string.Join(", ", activeSide.Hand.Select(p => p.InstanceId))}" );
                return;
            }
            // 2. Validation passed! Pass the execution down to the mechanic.
            match.PushAction( new NormalSummonAction(match.ActiveSideIndex, instanceId, laneIndex, playToFront));
        }
    }
}



