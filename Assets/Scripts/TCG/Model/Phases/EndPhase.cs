using TCG.Model.Core;

using UnityEngine;

namespace TCG.Model.Phases
{
    public class EndPhase : TurnPhase
    {
        public static readonly EndPhase Instance = new EndPhase();

        public EndPhase() : base()
        {
        
        }
    
        public override void Enter(Match match)
        {
            Debug.Log("End Phase Enter");
        }

        public override void Execute(Match match)
        {
            Debug.Log("End Phase Update");
            match.PassTurn();
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