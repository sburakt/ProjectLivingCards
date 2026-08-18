using System.Data.Common;
using TCG.Model.Actions;
using TCG.Model.Core;

using UnityEngine;

namespace TCG.Model.Phases
{
    public class EndPhase : TurnPhase
    {
        public static readonly EndPhase Instance = new EndPhase();

        public enum EndSteps
        {
            Restore,
            PostRestore,
            Finished,
        }

        public EndPhase() : base()
        {
        
        }
    
        public override void Enter(Match match)
        {
            match.CurrentPhaseStep = 0;
            Debug.Log("End Phase Enter");
        }

        public override void Execute(Match match)
        {
            Debug.Log("End Phase Update");
            EndSteps currentBattleStep = (EndSteps)match.CurrentPhaseStep;
            switch (currentBattleStep)
            {
                case EndSteps.Restore:
                    // no restoring the def for now to test if game feels faster/better this way
                    //match.PushAction(new RestoreSideDefenseAction(match.ActiveSideIndex));
                    match.CurrentPhaseStep = (int)EndSteps.PostRestore;
                    break;

                case EndSteps.PostRestore:
                    Debug.Log("End Phase PostRestore");
                    // events maybe 
                    match.CurrentPhaseStep = (int)EndSteps.Finished;
                    break;
                case EndSteps.Finished:
                    match.PassTurn();
                    break;
            }
        }

        public override void Exit(Match match)
        {
            Debug.Log("End Phase Exit");
        }

        public override TurnPhase GetNextPhase()
        {
            return null;
        }
    
    
    }
}