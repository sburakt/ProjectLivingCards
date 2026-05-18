using TCG.Model.Core;
using TCG.Model.Actions;

using UnityEngine;

namespace TCG.Model.Phases
{
    public class BattlePhase : TurnPhase
    {
        public static readonly BattlePhase Instance = new BattlePhase();

        public enum BattleSteps
        {
            PreBattle,
            Lane0,
            Lane1,
            Lane2,
            PostBattle,
            Finished,
        }

        public BattlePhase() : base()
        {
        }
    
        public override void Enter(Match match)
        {
            match.CurrentPhaseStep = 0;
            Debug.Log("Battle Phase Enter");
        }

        public override void Execute(Match match)
        {
            BattleSteps currentBattleStep = (BattleSteps)match.CurrentPhaseStep;
            switch (currentBattleStep)
            {
                case BattleSteps.PreBattle:
                    Debug.Log("Battle Phase PreBattle");
                    match.CurrentPhaseStep = (int)BattleSteps.Lane0;
                    break;

                case BattleSteps.Lane0:
                    match.PushAction(new StandardLaneBattleAction(0,match.ActiveSideIndex));
                    match.CurrentPhaseStep = (int)BattleSteps.Lane1;
                    break;

                case BattleSteps.Lane1:
                    match.PushAction(new StandardLaneBattleAction(1,match.ActiveSideIndex));
                    match.CurrentPhaseStep = (int)BattleSteps.Lane2;
                    break;

                case BattleSteps.Lane2:
                    match.PushAction(new StandardLaneBattleAction(2,match.ActiveSideIndex));
                    match.CurrentPhaseStep = (int)BattleSteps.PostBattle;
                    break;
            
                case BattleSteps.PostBattle:
                    Debug.Log("Battle Phase PostBattle");
                    match.CurrentPhaseStep = (int)BattleSteps.Finished;
                    break;
            
                case BattleSteps.Finished:
                    match.AdvancePhase();
                    break;
            }
        }

        public override void Exit(Match match)
        {
            Debug.Log("Battle Phase Exit");
        }

        public override TurnPhase GetNextPhase()
        {
            return EndPhase.Instance;
        }

    }
}