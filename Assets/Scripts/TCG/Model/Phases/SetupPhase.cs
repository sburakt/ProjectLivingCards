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
            int groupId = match.ResolveGroupId(Match.UNASSIGNED_GROUP_ID);
            for (int i = 0; i < 4; i++)
            {
                match.PushAction(new DrawAction(1,groupId));
            }

            for (int i = 0; i < 4; i++)
            {
                match.PushAction(new DrawAction(0,groupId));
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