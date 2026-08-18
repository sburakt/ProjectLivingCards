using System.Collections.Generic;
using TCG.Model.Core;

namespace TCG.Model.Phases
{
    public abstract class TurnPhase
    {
        protected TurnPhase()
        {
        }

        public virtual void Enter(Match match)
        {
        }

        public virtual void Execute(Match match)
        {
        }

        public virtual void Exit(Match match)
        {
        }

        //moves can be made only in main phase but if changes in the future it may be hard to implement without per phase legal move method
        public virtual List<PlayerMove> GetLegalMoves(Match match)
        {
            return new List<PlayerMove>();
        }
        
        // public virtual void ReceiveInput(string input)
        // {
        //     recievedInput = input;
        // }

        public abstract TurnPhase GetNextPhase();
    }
}