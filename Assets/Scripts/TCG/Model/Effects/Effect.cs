using TCG.Model.Core;

namespace TCG.Model.Effects
{
    public abstract class Effect 
    {
        protected readonly int OwnerInstanceId;

        protected Effect(int ownerInstanceId)
        {
            OwnerInstanceId = ownerInstanceId;
        }

        public abstract bool ShouldTerminate(Match match);

        public virtual void Terminate(Match match)
        {
            //do nothing
            // for effect that need to undo their effect on the match
        }
    }
}