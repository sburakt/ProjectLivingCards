using TCG.Model.Core;
using TCG.Model.Actions;

namespace TCG.Model.Phases
{
    public class SetupPhase : TurnPhase
    {
        public static readonly SetupPhase Instance = new SetupPhase();

        public override void Enter(Match match)
        {
            base.Enter(match);
        }

        // single setup phase handles both players
        // split into per player setup if needed
        public override void Execute(Match match)
        {
            for (int i = 0; i < 4; i++)
            {
                match.PushAction(new DrawCardAction(1));
            }

            for (int i = 0; i < 4; i++)
            {
                match.PushAction(new DrawCardAction(0));
            }
            
            match.AdvancePhase();
        }

        public override void Exit(Match match)
        {
            base.Exit(match);
        }

        public override TurnPhase GetNextPhase()
        {
            return DrawPhase.Instance;
        }
    }
}