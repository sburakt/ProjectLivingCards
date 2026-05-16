using TCG.Model.Core;

namespace TCG.Model.Phases
{
    public abstract class TurnPhase
    {
        //protected string recievedInput = "";
        protected TurnPhase()
        {
        }

        public virtual void Enter(Match match) {}

        public virtual void Execute(Match match)
        {
        }

        public virtual void Exit(Match match)
        {}

        // public virtual void ReceiveInput(string input)
        // {
        //     recievedInput = input;
        // }

        public abstract TurnPhase GetNextPhase();
    }
}