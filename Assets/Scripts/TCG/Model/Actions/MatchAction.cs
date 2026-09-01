using TCG.Model.Core;

namespace TCG.Model.Actions
{
    public abstract class MatchAction
    {
        public int GroupId { get; protected set; }

        public abstract void Execute(Match match);
        
        public virtual void UpdateGroupID(Match match)
        {
            GroupId = match.ResolveGroupId(GroupId);
        }
    }
}

    /*
    public struct ActionData
    {
        public ActionType ActionType;
        public int Int1;
        public int Int2;
        public int Int3;
        public int Int4;
        public int Int5;
        public Position Pos1;
        public Position Pos2;
        public Position Pos3;
        public Position Pos4;
    }

    enum ActionType
    {
        //all actions here
    }
    */

    //possible use case for tagged union profile gc impact on enemy ai algo
