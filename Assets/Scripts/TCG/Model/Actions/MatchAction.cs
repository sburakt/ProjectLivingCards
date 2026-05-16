using TCG.Model.Core;

namespace TCG.Model.Actions
{
    public abstract class MatchAction
    {
        public abstract void Execute(Match match);
    }
}