using TCG.Model.Core;
using TCG.Model.Actions;
using TCG.Model.Events;
using UnityEngine;

namespace TCG.Model.Phases
{
    public class DrawPhase : TurnPhase
    {
        public static readonly DrawPhase Instance = new DrawPhase();
        private enum DrawStep
        {
            PreDraw,
            Draw,
            PostDraw,
            Finished
        };
    
        public DrawPhase() : base() { }

        public override void Enter(Match match)
        {
            Debug.Log("Draw Phase Enter");
            match.CurrentPhaseStep = 0;
        }

        public override void Execute(Match match)
        {
            DrawStep currentStep = (DrawStep)match.CurrentPhaseStep;
            switch (currentStep)
            {
                case DrawStep.PreDraw:
                    Debug.Log("Draw Phase PreDraw");
                    // todo use event predraw 
                    match.CurrentPhaseStep = (int)DrawStep.Draw;
                    break;
                
                case DrawStep.Draw:
                    Debug.Log("Draw Phase Draw");
                    match.PushAction(new DrawCardAction(match.ActiveSideIndex));
                    match.CurrentPhaseStep = (int)DrawStep.PostDraw;
                    break;

                case DrawStep.PostDraw:
                    Debug.Log("Draw Phase PostDraw");
                    // todo use event postdraw
                    match.CurrentPhaseStep = (int)DrawStep.Finished;
                    break;

                
                case DrawStep.Finished:
                    Debug.Log("Draw Phase Finished");
                    match.AdvancePhase();
                    match.EnqueueEvent(new MatchEvent()
                    {
                        Type = MatchEventType.TurnStarted,
                        SourceId = match.ActiveSideIndex
                    });
                    break;
            }
        }

        public override void Exit(Match match)
        {
            Debug.Log("Draw Phase Exit");
        }

        public override TurnPhase GetNextPhase()
        {
            return MainPhase.Instance;
        }
    }
}